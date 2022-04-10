using System.Threading.Tasks;

namespace PL.Console.Interfaces
{
    public interface IAuthorization
    {
        Task<bool> AuthorizeUserAsync();
    }
}
