using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces;
using Core;
using DAL.Abstractions.Interfaces;

namespace BLL.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public PasswordService(ICurrentUser currentUser, IUnitOfWork unitOfWork)
        {
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ChangePassword(string oldPassword, string newPassword)
        {
            var user = _currentUser.User;

            var result = IsPasswordCorrect(user, oldPassword) && await SetPassword(user, newPassword);

            try
            {
                _unitOfWork.CreateTransaction();

                _unitOfWork.UserRepository.Update(user);

                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
            }
            

            return result;
        }

        public async Task<bool> SetPassword(User user, string password, bool toSaveUser = false)
        {
            if (HasPasswordCorrectFormat(user.Email, password))
            {
                user.Password = GetHash(password);

                if (toSaveUser)
                {
                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Save();
                }

                return true;
            }

            return false;
        }

        public bool HasPasswordCorrectFormat(string email, string password)
        {
            if (!string.IsNullOrWhiteSpace(password) && password.Length is >= 8 and <= 24)
            {
                Regex regex = new("([A-Za-z0-9!\"#%&'()*,\\-./:;?@[\\\\\\]_{}¡«·»¿;])*");
                var searchedString = regex.Match(password).Value;

                if (searchedString == password && email != password)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsPasswordCorrect(User user, string password)
        {
            var passHash = GetHash(password);

            return passHash == user.Password;
        }

        private string GetHash(string password)
        {
            var passBytes = Encoding.UTF8.GetBytes(password);
            var passHash = SHA256.Create().ComputeHash(passBytes);

            return Encoding.UTF8.GetString(passHash);
        }
    }
}
