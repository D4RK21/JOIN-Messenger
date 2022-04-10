using System.Collections.Generic;
using System.Linq;
using BLL.Abstractions.Interfaces;
using Core;
using PL.Console.Interfaces;

namespace PL.Console.RoomsControl
{
    public class Invitation : IInvitation
    {
        private readonly IUrlInvitationService _invitationService;
        private readonly IUserValidator _userValidator;

        public Invitation(IUrlInvitationService invitationService, IUserValidator userValidator)
        {
            this._invitationService = invitationService;
            this._userValidator = userValidator;
        }

        public void EnterRoomWithUrl()
        {
            System.Console.WriteLine("Enter invitation url: ");
            var url = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(url))
            {
                if (_invitationService.JoinByUrl(url).Result)
                {
                    System.Console.WriteLine("You successfully entered the room");
                }
                else
                {
                    System.Console.WriteLine("You dont have access try again later");
                }
            }
            else
            {
                System.Console.WriteLine("Something went wrong, try later");
            }
        }

        public void InviteToRoomWithUrl(Room room)
        {
            System.Console.WriteLine(
                "If you want to create url for invitation press \"u\", if you want to invite specified user/users press \"s\"");
            var userKey = System.Console.ReadLine();
            while (userKey != "u" && userKey != "s")
            {
                System.Console.Write("Unknown key, please enter again: ");
                userKey = System.Console.ReadLine();
            }

            if (userKey == "u")
            {
                var url = _invitationService.InviteUsersByUrl(room).Result;
                System.Console.WriteLine(url);
            }

            if (userKey == "s")
            {
                System.Console.WriteLine(
                    "Enter username / email that you want to invite(if many users enumerate them with \"space\")");
                var userNames = System.Console.ReadLine()?.Split(" ");
                var usersToInvite = new List<string>();
                var errors = new List<string>();
                foreach (var name in userNames)
                {
                    var validationsResult = _userValidator.ValidateUserNameOrEmail(name).Result;
                    if (validationsResult)
                    {
                        usersToInvite.Add(name);
                    }
                    else
                    {
                        errors.Add(name);
                    }
                }

                _invitationService.InviteUsersByEmailWithUrlAsync(room, usersToInvite).Wait();

                if (errors.Any())
                {
                    System.Console.WriteLine("Invitations have not been send to such users:");
                    foreach (var error in errors)
                    {
                        System.Console.WriteLine(error);
                    }
                }
            }
        }
    }
}
