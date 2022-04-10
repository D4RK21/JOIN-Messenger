using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using BLL.Abstractions.Interfaces;
using PL.Console.Interfaces;

namespace PL.Console.RoomsControl
{
    public class RoomsControl : IRoomsControl
    {
        private readonly IRoomService _roomService;
        private readonly IUserService _userService;
        private readonly IInvitation _invitation;
        private readonly IRoleControl _roleControl;
        private readonly ITextChannelControl _textChannelControl;
        private readonly IPersonalChatControl _personalChatControl;


        public RoomsControl(IRoomService roomService, IUserService userService, IInvitation invitation,
            IRoleControl roleControl, ITextChannelControl textChannelControl, IPersonalChatControl personalChatControl)
        {
            this._roomService = roomService;
            this._userService = userService;
            this._invitation = invitation;
            this._roleControl = roleControl;
            this._textChannelControl = textChannelControl;
            this._personalChatControl = personalChatControl;
        }

        public async Task ShowUserRooms()
        {
            var userRooms = await _userService.GetUserRooms();

            if (userRooms.Count == 0)
            {
                System.Console.WriteLine("You have no rooms");
            }
            else
            {
                System.Console.WriteLine("Your rooms: ");

                for (var i = 0; i < userRooms.Count; i++)
                {
                    System.Console.WriteLine($"\t{i + 1}) {userRooms[i].RoomName}");
                }
            }

            ChooseAction(userRooms);
        }

        private void ChooseAction(List<Room> userRooms)
        {
            string userInput;
            do
            {
                System.Console.Write(
                    "If you want to choose room - type its number or if you want to create room - enter \"create\"" +
                    " or if you want to join - enter \"join\" " +
                    "or if you want to show personal chats - enter \"pc\": ");
                userInput = System.Console.ReadLine()?.Trim();
            } while (string.IsNullOrWhiteSpace(userInput) && userInput != "create" && userInput != "join" &&
                     userInput != "pc" && !int.TryParse(userInput, out _));

            if (userInput == "create")
            {
                CreateRoom();
            }
            else if (userInput == "join")
            {
                _invitation.EnterRoomWithUrl();
            }
            else if (userInput == "pc")
            {
                _personalChatControl.DoAction();
            }
            else if (int.TryParse(userInput, out var roomNumber) && userRooms.Count >= roomNumber)
            {
                ChooseRoomAction(userRooms[roomNumber - 1]);
            }
            else
            {
                System.Console.WriteLine("Error! Please, try again later!");
            }
        }

        public bool ChooseRoomAction(Room room)
        {
            string action;
            do
            {
                System.Console.WriteLine("What do you want to do? (\"delete\" or \"set up\" or \"leave\" or " +
                                         "\"notification\" or \"invite\" or \"roles\" or \"text channels\")");
                action = System.Console.ReadLine()?.Trim();
            } while (string.IsNullOrWhiteSpace(action));

            switch (action)
            {
                case "delete":
                    return DeleteRoom(room);
                case "set up":
                    return SetUpRoom(room);
                case "leave":
                    return LeaveRoom(room);
                case "notification":
                    return ChangeRoomNotifications(room);
                case "invite":
                    _invitation.InviteToRoomWithUrl(room);
                    return true;
                case "roles":
                    _roleControl.ViewRolesInTheRoom(room).Wait();
                    return true;
                case "text channels":
                    _textChannelControl.TextChannelChoice(room);
                    return true;
            }

            System.Console.WriteLine("Error! Please, try again later!");

            return false;
        }

        private bool CreateRoom()
        {
            string name;
            do
            {
                System.Console.WriteLine("Enter room's name:");
                name = System.Console.ReadLine()?.Trim();
            } while (string.IsNullOrWhiteSpace(name));

            string description;
            do
            {
                System.Console.WriteLine("Enter room's description:");
                description = System.Console.ReadLine()?.Trim();
            } while (string.IsNullOrWhiteSpace(description));

            var roomId = _roomService.CreateRoom(name, description).Result;

            if (roomId != null)
            {
                System.Console.WriteLine($"Room successfully created! Its Id is: {roomId}");

                return true;
            }

            System.Console.WriteLine("Error! Please, try again later!");

            return false;
        }

        private bool DeleteRoom(Room room)
        {
            if (_roomService.DeleteRoom(room).Result)
            {
                System.Console.WriteLine("Room successfully deleted!");

                return true;
            }

            System.Console.WriteLine("Error! Please, try again later!");

            return false;
        }

        private bool SetUpRoom(Room room)
        {
            var actions = new[] {"name", "description", "both"};
            string action;

            do
            {
                System.Console.WriteLine("What do you want to change? (\"name\" or \"description\" or \"both\")");
                action = System.Console.ReadLine()?.Trim();
            } while (string.IsNullOrWhiteSpace(action) || !actions.Contains(action));

            string name = null;
            string description = null;

            if (action == "name" || action == "both")
            {
                System.Console.WriteLine("Enter new name:");
                name = System.Console.ReadLine();
            }

            if (action == "description" || action == "both")
            {
                System.Console.WriteLine("Enter new description:");
                description = System.Console.ReadLine();
            }

            if (_roomService.ChangeRoomSettings(room, name, description).Result)
            {
                System.Console.WriteLine("Room settings successfully changed!");

                return true;
            }

            System.Console.WriteLine("Error! Please, try again later!");

            return false;
        }

        private bool LeaveRoom(Room room)
        {
            if (_userService.LeaveRoom(room).Result)
            {
                System.Console.WriteLine("Successfully leaved the room!");
                return true;
            }

            return false;
        }

        private bool ChangeRoomNotifications(Room room)
        {
            string userResponse;

            do
            {
                System.Console.Write("Do you want to receive notifications from this room (\"yes\" or \"no\")? ");
                userResponse = System.Console.ReadLine()?.Trim();
            } while (string.IsNullOrWhiteSpace(userResponse) && userResponse != "yes" && userResponse != "no");

            switch (userResponse)
            {
                case "yes":
                    return _userService.SwitchNotifications(room, true).Result;
                case "no":
                    return _userService.SwitchNotifications(room, false).Result;
            }

            System.Console.WriteLine("Error! Please, try again later!");

            return false;
        }
    }
}
