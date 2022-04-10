using System.Threading.Tasks;

namespace BLL.Abstractions.Interfaces
{
    public interface IRegistrationService
    {
        string Code { get; set; }

        Task RegisterAsync(string userMail, string name, string surname, string nickName, string password,
            bool isVerified = false);
    }
}
