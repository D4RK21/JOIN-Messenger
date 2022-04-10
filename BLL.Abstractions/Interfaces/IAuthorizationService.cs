using System.Threading.Tasks;
using Core;

namespace BLL.Abstractions.Interfaces
{
    public interface IAuthorizationService
    {
        Task<bool> CheckUserDataForAuthAsync(string usernameOrEmail, string password);

        Task<string> GetEmailByUsernameOrEmail(string usernameOrEmail);

        Task<bool> IsLastAuthWasLongAgo(User user, int numberOfDays);

        Task UpdateLastAuth(User user);

        Task<User> GetInfoAboutUser(string usernameOrEmail);
    }
}
