using System.Threading.Tasks;
using Core;

namespace BLL.Abstractions.Interfaces
{
    public interface IPasswordService
    {
        Task<bool> ChangePassword(string oldPassword, string newPassword);

        Task<bool> SetPassword(User user, string password, bool toSaveUser = false);

        bool HasPasswordCorrectFormat(string email, string password);

        bool IsPasswordCorrect(User user, string password);
    }
}
