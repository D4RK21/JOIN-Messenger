using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace BLL.Abstractions.Interfaces
{
    public interface IRoleService
    {
        Task<bool> SetUpRole(Room room, Role role, IDictionary<string, bool?> permissions);

        Task<Role> CreateNewRole(Room room, string name);

        Task<bool> DeleteRole(Room room, Role role);

        Task<List<Role>> GetAllRolesInRoom(Room room);
        
        Task<bool> SetRoleToUser(Room room, User user, Role role);

        Task<Dictionary<string, string>> GetRolesOfUsers(int roomId);
    }
}
