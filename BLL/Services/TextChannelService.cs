using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces;
using Core;
using DAL.Abstractions.Interfaces;

namespace BLL.Services
{
    public class TextChannelService : ITextChannelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public TextChannelService(IUnitOfWork unitOfWork, ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> EditTextChannel(TextChannel textChannel, Room room, string name = null,
            string description = null, bool? isAdmin = null)
        {
            var user = _currentUser.User;
            
            if (!await CanManageChannels(room, user) ||
                (!await CanUseAdminChannels(room, user) && (textChannel.IsAdminChannel || isAdmin is true)))
            {
                return false;
            }

            try
            {
                if (name != null)
                {
                    textChannel.ChannelName = name;
                }

                if (description != null)
                {
                    textChannel.ChannelDescription = description;
                }

                if (isAdmin.HasValue)
                {
                    textChannel.IsAdminChannel = isAdmin.Value;
                }

                _unitOfWork.TextChannelRepository.Update(textChannel);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CreateTextChannel(Room room, string name, string description, bool isAdmin = false)
        {
            var user = _currentUser.User;

            if (!await CanManageChannels(room, user) || (isAdmin && !await CanUseAdminChannels(room, user)))
            {
                return false;
            }

            var newTextChannel = new TextChannel()
            {
                ChannelName = name, ChannelDescription = description, IsAdminChannel = isAdmin
            };
            
            room.TextChannels.Add(newTextChannel);
            
            _unitOfWork.CreateTransaction();
            try
            {
                await _unitOfWork.TextChannelRepository.CreateAsync(newTextChannel);
                _unitOfWork.RoomRepository.Update(room);
                
                _unitOfWork.Commit();
                
                return true;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                return false;
            }
        }

        public async Task<List<TextChannel>> GetTextChannels(Room room)
        {
            var result = new List<TextChannel>();
            var user = _currentUser.User;

            foreach (var chat in room.TextChannels)
            {
                if ((chat.IsAdminChannel && await CanUseAdminChannels(room, user)) || !chat.IsAdminChannel)
                {
                    result.Add(chat);
                }
            }

            return result;
        }

        public async Task<bool> DeleteTextChannel(TextChannel textChannel, Room room)
        {
            if (!await CanManageChannels(room, _currentUser.User))
            {
                return false;
            }
            
            room.TextChannels.Remove(textChannel);

            _unitOfWork.CreateTransaction();
            try
            {
                _unitOfWork.TextChannelRepository.Delete(textChannel);
                _unitOfWork.RoomRepository.Update(room);

                _unitOfWork.Commit();
                
                return true;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                return false;
            }
        }

        public async Task<bool> CanManageChannels(Room room, User user = null)
        {
            if (user == null)
            {
                user = _currentUser.User;
            }

            var participant = room.Participants.FirstOrDefault(participant => participant.User.Id == user.Id);
            var userRole = participant?.Role;

            return userRole!.CanManageChannels;
        }

        public async Task<bool> CanUseAdminChannels(Room room, User user = null)
        {
            if (user == null)
            {
                user = _currentUser.User;
            }

            var participant = room.Participants.FirstOrDefault(participant => participant.User.Id == user.Id);
            var userRole = participant?.Role;

            return userRole!.CanUseAdminChannels;
        }
    }
}
