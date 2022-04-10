using System.Threading.Tasks;
using Core;

namespace PL.Console.Interfaces
{
    public interface IRoleControl
    {
        Task ViewRolesInTheRoom(Room room);
    }
}
