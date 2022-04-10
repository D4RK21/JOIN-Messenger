using System.Collections.Generic;
using System.Linq;
using BLL.Abstractions.Interfaces;
using Core;
using PL.Console.Interfaces;

namespace PL.Console.RoomsControl
{
    public class TextChannelControl : ITextChannelControl
    {
        private readonly ITextChannelService _textChannelService;

        public TextChannelControl(ITextChannelService textChannelService)
        {
            _textChannelService = textChannelService;
        }

        public bool TextChannelChoice(Room room)
        {
            var channelList = _textChannelService.GetTextChannels(room).Result;
            var counter = 1;

            System.Console.WriteLine($"Text channels in {room.RoomName}:");

            if (channelList.Count == 0)
            {
                System.Console.WriteLine("\tThere is no text channels in room");
            }

            foreach (var channel in channelList)
            {
                System.Console.WriteLine($"\t{counter}) {channel.ChannelName}");
                counter++;
            }

            return TextChannelAction(room, channelList);
        }

        private bool TextChannelAction(Room room, List<TextChannel> channelList)
        {
            var actions = new string[] {"create"};

            string action;
            var outAction = -1;
            do
            {
                System.Console.Write($"What do you want to do? ({string.Join(" or ", actions)} or type its number): ");
                action = System.Console.ReadLine()?.Trim();
                _ = int.TryParse(action, out outAction);
            } while (string.IsNullOrWhiteSpace(action) || (!actions.Contains(action) && outAction > channelList.Count &&
                                                           outAction <= 0));

            if (action == "create")
            {
                if (CreateTextChannel(room))
                {
                    System.Console.WriteLine("Text channel created successfully!");
                    return true;
                }

                System.Console.WriteLine("Error! Please, try again later!");

                return false;
            }

            return ChannelsAction(room, channelList[outAction - 1]);
        }

        private bool CreateTextChannel(Room room)
        {
            if (!_textChannelService.CanManageChannels(room).Result)
            {
                return false;
            }

            string name;
            do
            {
                System.Console.Write("Enter text channel name: ");
                name = System.Console.ReadLine()?.Trim();
            } while (string.IsNullOrWhiteSpace(name));

            string description;
            do
            {
                System.Console.Write("Enter text channel description: ");
                description = System.Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(description));

            if (!_textChannelService.CanUseAdminChannels(room).Result)
            {
                return _textChannelService.CreateTextChannel(room, name, description, false).Result;
            }

            var isAdmin = false;

            string admin;
            do
            {
                System.Console.Write("Do you want to this channel be private (\"t\" or \"f\")? ");
                admin = System.Console.ReadLine()?.Trim();
            } while (admin != "t" && admin != "f");

            if (admin == "t")
            {
                isAdmin = true;
            }

            return _textChannelService.CreateTextChannel(room, name, description, isAdmin).Result;
        }

        private bool ChannelsAction(Room room, TextChannel textChannel) //TODO: delete empty method
        {
            var actions = new string[] {"edit", "delete"};

            string action;
            do
            {
                System.Console.Write($"What do you want to do? ({string.Join(" or ", actions)}): ");
                action = System.Console.ReadLine();
            } while (action == null || !actions.Contains(action));

            switch (action)
            {
                case "edit":
                    return EditTextChannel(room, textChannel);
                case "delete":
                    return DeleteTextChannel(room, textChannel);
            }

            return true;
        }

        private bool DeleteTextChannel(Room room, TextChannel textChannel)
        {
            if (!_textChannelService.CanManageChannels(room).Result ||
                (!_textChannelService.CanUseAdminChannels(room).Result && textChannel.IsAdminChannel))
            {
                return false;
            }

            return _textChannelService.DeleteTextChannel(textChannel, room).Result;
        }

        private bool EditTextChannel(Room room, TextChannel textChannel)
        {
            if (!_textChannelService.CanManageChannels(room).Result ||
                (!_textChannelService.CanUseAdminChannels(room).Result && textChannel.IsAdminChannel))
            {
                return false;
            }

            System.Console.Write("Enter new channel name (if you  don't want to change it - just press Enter): ");
            var name = System.Console.ReadLine()?.Trim();
            name = name == string.Empty ? null : name;

            System.Console.Write(
                "Enter new channel description (if you  don't want to change it - just press Enter): ");
            var description = System.Console.ReadLine()?.Trim();
            description = description == string.Empty ? null : description;

            bool? isAdmin = null;
            if (_textChannelService.CanUseAdminChannels(room).Result)
            {
                var actions = new string[] {"y", "n", null};

                string admin;
                do
                {
                    System.Console.Write($"Do you want to do it private? ({string.Join(" or ", actions)}or if you " +
                                         $"don't want to change it - just press Enter): ");
                    admin = System.Console.ReadLine()?.Trim();
                    admin = admin == string.Empty ? null : admin;
                } while (!actions.Contains(admin));

                switch (admin)
                {
                    case "y":
                        isAdmin = true;
                        break;
                    case "n":
                        isAdmin = false;
                        break;
                }
            }

            if (_textChannelService.EditTextChannel(textChannel, room, name, description, isAdmin).Result)
            {
                System.Console.WriteLine("Text channel edited successfully!");
                return true;
            }

            System.Console.WriteLine("Error! Please, try again later!");
            return false;
        }
    }
}
