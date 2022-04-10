using System;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces;
using Core;
using DAL.Abstractions.Interfaces;

namespace BLL.Services
{
    public class RegistrationService : IRegistrationService
    {
        public string Code { get; set; }

        private readonly IPasswordService _passwordService;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public RegistrationService(IPasswordService passwordService, ICurrentUser currentUser, IUnitOfWork unitOfWork)
        {
            this._passwordService = passwordService;
            this._currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task RegisterAsync(string userMail, string name, string surname, string nickName, string password,
            bool isVerified = false)
        {
            var user = new User()
            {
                Email = userMail,
                FirstName = name,
                LastName = surname,
                UserName = nickName,
                IsVerified = isVerified,
                LastAuth = DateTime.Now
            };

            await _passwordService.SetPassword(user, password);

            await _unitOfWork.UserRepository.CreateAsync(user);
            _unitOfWork.Save();

            _currentUser.User = user;
        }
    }
}
