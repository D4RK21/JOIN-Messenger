using System;
using System.Collections.Generic;
using System.Globalization;
using BLL.Abstractions.Interfaces;
using Core;
using DAL.Abstractions.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace BLL.Services
{
    public class UrlInvitationService : IUrlInvitationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IMailWorker _mailWorker;
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;

        public UrlInvitationService(ICurrentUser currentUser,
            IMailWorker mailWorker, IUserService userService, IOptions<AppSettings> appSettings, IUnitOfWork unitOfWork)
        {
            this._currentUser = currentUser;
            this._mailWorker = mailWorker;
            this._userService = userService;
            this._unitOfWork = unitOfWork;
            this._appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task InviteUsersByEmailWithUrlAsync(Room room, List<string> users)
        {
            var currentTime = DateTime.Now;
            var expirationTime = currentTime.AddMinutes(15).ToString(CultureInfo.InvariantCulture);

            var url = new InviteLink
            {
                Room = room, Url = string.Concat(_appSettings.Domain, Guid.NewGuid().ToString().AsSpan(0, 7)),
            };

            var listOfUsersLinks = new List<InviteLinksUsers>();

            _unitOfWork.CreateTransaction();
            try
            {
                foreach (var user in users)
                {
                    var inviteUser = await _userService.GetUserByUserNameOrEmail(user);
                    var usersLink = new InviteLinksUsers() {User = inviteUser, InviteLink = url};
                    if (room.Participants.Any(info => info.User.Id == inviteUser.Id))
                    {
                        return;
                    }

                    listOfUsersLinks.Add(usersLink);
                    _unitOfWork.InviteLinksUsersRepository.Update(usersLink);

                    await _mailWorker.SendInvitationEmailAsync(room, url.Url, inviteUser.Email); //ToDO bool
                }

                url.User = listOfUsersLinks;
                url.ExpirationTime = expirationTime;
                await _unitOfWork.InviteLinkRepository.CreateAsync(url);

                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
            }
        }

        public async Task<string> InviteUsersByUrl(Room room)
        {
            var currentTime = DateTime.Now;
            var expirationTime = currentTime.AddHours(5).ToString(CultureInfo.InvariantCulture);
            var url = new InviteLink
            {
                Url = string.Concat(_appSettings.Domain, Guid.NewGuid().ToString().AsSpan(0, 6)),
                ExpirationTime = expirationTime,
                User = null,
                Room = room
            };

            await _unitOfWork.InviteLinkRepository.CreateAsync(url);

            return url.Url;
        }

        public async Task<bool> JoinByUrl(string url)
        {
            var urlFromDb =
                _unitOfWork.InviteLinkRepository.FindByCondition(urls => urls.Url == url,
                    InviteLink.Selector);

            var urlsEnumerable = urlFromDb.ToList();
            if (urlsEnumerable.Any())
            {
                var responseUrl = urlsEnumerable.First();
                var now = DateTime.Now;
                var expirationTime = DateTime.Parse(responseUrl.ExpirationTime, CultureInfo.InvariantCulture);

                if (responseUrl.User == null ||
                    responseUrl.User.Select(x => x.Id).Contains(_currentUser.User.Id) && expirationTime >= now)
                {
                    var rooms =
                        _unitOfWork.RoomRepository.FindByCondition(room => room.Id == responseUrl.Room.Id, Room.Selector);

                    var room = rooms.FirstOrDefault();
                    var participantInfo = new ParticipantInfo()
                    {
                        Notifications = true, User = _currentUser.User, Role = room?.BaseRole
                    };

                    if (room != null && room.Participants.All(info => info.User.Id != participantInfo.User.Id))
                    {
                        room.Participants.Add(participantInfo);
                    }

                    _unitOfWork.RoomRepository.Update(room);
                    _unitOfWork.Save();

                    return true;
                }
            }

            return false;
        }
    }
}
