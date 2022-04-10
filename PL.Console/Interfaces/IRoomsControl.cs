using System.Threading.Tasks;
using Core;

namespace PL.Console.Interfaces
{
    public interface IRoomsControl
    {
        Task ShowUserRooms();
        
        bool ChooseRoomAction(Room room);
    }
}
