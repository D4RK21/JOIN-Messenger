using System.Threading.Tasks;

namespace PL.Console.Interfaces
{
    public interface IResetPasswordControl
    {
        Task<bool> ResetUserPasswordAsync();
    }
}
