using Core;

namespace PL.Console.Interfaces
{
    public interface IInvitation
    {
        void EnterRoomWithUrl();
        
        void InviteToRoomWithUrl(Room room);
    }
}
