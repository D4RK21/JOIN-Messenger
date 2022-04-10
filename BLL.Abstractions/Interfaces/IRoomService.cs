using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace BLL.Abstractions.Interfaces
{
    public interface IRoomService
    {
        Task<int> CreateRoom(string name, string description);
        
        Task<bool> DeleteRoom(Room room);
        
        Task<bool> ChangeRoomSettings(Room room, string name, string description);
        
        Task<List<User>> GetParticipantsOfRoom(int roomId);
    }
}
