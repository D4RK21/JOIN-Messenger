using System;
using BLL.Abstractions.Interfaces;
using DAL.Abstractions.Interfaces;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BLL.Helpers
{
    public class UserValidator : IUserValidator
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> IsEmailValid(string email)
        {
            try
            {
                var users = _unitOfWork.UserRepository.FindByCondition(user => user.Email == email);
                if (users.Any())
                {
                    return 1;
                }

                _ = new MailAddress(email);

                return 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }


        public async Task<bool> ValidateUserNick(string nick)
        {
            var result = _unitOfWork.UserRepository.FindByCondition(user => user.UserName == nick);
            if (result.Any())
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ValidateUserNameOrEmail(string userName)
        {
            var result = _unitOfWork.UserRepository
                .FindByCondition(user => user.UserName == userName || user.Email == userName);
            
            return result.Any();
        }
    }
}
