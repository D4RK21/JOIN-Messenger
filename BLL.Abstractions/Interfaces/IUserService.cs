using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace BLL.Abstractions.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByUserNameOrEmail(string userName);
        
        Task<bool> LeaveRoom(Room room);
        
        Task<bool> SwitchNotifications(Room room, bool stateOnOrOff);
        
        bool IsUserVerified(User user);
        
        Task<List<Room>> GetUserRooms();
        
        Task<Role> GetRoleInRoom(Room room);
        
        Task ChangeUserNames(string firstName, string lastName);
    }
}
