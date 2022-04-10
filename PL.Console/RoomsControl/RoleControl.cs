using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces;
using Core;
using PL.Console.Interfaces;

namespace PL.Console.RoomsControl
{
    public class RoleControl : IRoleControl
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public RoleControl(IRoleService roleService, IUserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }

        public async Task ViewRolesInTheRoom(Room room)
        {
            var roles = await _roleService.GetAllRolesInRoom(room);

            if (roles.Count == 0)
            {
                System.Console.WriteLine("There is no roles in room");
            }
            else
            {
                System.Console.WriteLine("Roles in this room: ");

                for (var i = 0; i < roles.Count; i++)
                {
                    System.Console.WriteLine($"\t{i + 1}) {roles[i].RoleName}");
                }
            }

            string userInput;
            do
            {
                System.Console.Write(
                    "If you want to choose role - type its number or if you want to create new one - enter \"create\" or \"my role\" - to see your role or" +
                    " \"users roles\" - to see roles of all users in the room: ");
                userInput = System.Console.ReadLine()?.Trim();
            } while (string.IsNullOrWhiteSpace(userInput) && userInput != "create" && userInput != "my role" &&
                     userInput != "users roles" && !int.TryParse(userInput, out _));


            if (userInput == "create")
            {
                await CreateRole(room);
            }

            if (userInput == "my role")
            {
                await ViewMyRole(room);
            }

            if (userInput == "users roles")
            {
                await GetUsersRoles(room);
            }

            else if (int.TryParse(userInput, out var roleNumber) && roles.Count >= roleNumber)
            {
                await ChooseRoleAction(room, roles[roleNumber - 1]);
            }
        }

        private async Task<bool> CreateRole(Room room)
        {
            string roleName;
            do
            {
                System.Console.WriteLine("Enter role name: ");
                roleName = System.Console.ReadLine()?.Trim();
            } while (string.IsNullOrWhiteSpace(roleName));

            var role = await _roleService.CreateNewRole(room, roleName);

            if (role is null)
            {
                System.Console.WriteLine("Error! Please, try again later!");
                return false;
            }

            System.Console.WriteLine("Role successfully created");

            return await SetUpRole(room, role);
        }

        private async Task<bool> ViewMyRole(Room room) //TODO: change for permissions dict
        {
            try
            {
                var myRole = await _userService.GetRoleInRoom(room);

                System.Console.WriteLine($"Your role - {myRole.RoleName}");
                System.Console.WriteLine("Permissions:");
                System.Console.WriteLine($"\t-Can Pin: {myRole.CanPin}");
                System.Console.WriteLine($"\t-Can Invite: {myRole.CanInvite}");
                System.Console.WriteLine($"\t-Can Delete Others Messages: {myRole.CanDeleteOthersMessages}");
                System.Console.WriteLine($"\t-Can Moderate Participants: {myRole.CanModerateParticipants}");
                System.Console.WriteLine($"\t-Can Manage Roles: {myRole.CanManageRoles}");
                System.Console.WriteLine($"\t-Can Manage Channels: {myRole.CanManageChannels}");
                System.Console.WriteLine($"\t-Can Manage Room: {myRole.CanManageRoom}");
                System.Console.WriteLine($"\t-Can Use Admin Channels: {myRole.CanUseAdminChannels}");
                System.Console.WriteLine($"\t-Can View Audit Log: {myRole.CanViewAuditLog}");

                return true;
            }
            catch (Exception)
            {
                System.Console.WriteLine("Error! Please, try again later!");
                return false;
            }
        }

        private async Task<bool> ChooseRoleAction(Room room, Role role)
        {
            string userInput;
            do
            {
                System.Console.WriteLine(
                    "If you want to edit permissions - type \"edit\" or if you want to delete role - type \"delete\"");
                userInput = System.Console.ReadLine()?.Trim();
            } while (string.IsNullOrWhiteSpace(userInput) && userInput != "edit" && userInput != "delete");

            switch (userInput)
            {
                case "edit":
                    return await SetUpRole(room, role);
                case "delete":
                    return await DeleteRole(room, role);
            }

            return false;
        }

        private async Task<bool> SetUpRole(Room room, Role role)
        {
            var permissions = new Dictionary<string, bool?>();

            foreach (var pair in role.Permissions)
            {
                string stringValue;
                do
                {
                    System.Console.Write($"Choose permission for {pair.Key} (\"t\", \"f\" or empty): ");
                    stringValue = System.Console.ReadLine();
                    stringValue = string.IsNullOrWhiteSpace(stringValue) ? null : stringValue;
                } while (stringValue != null && stringValue != "t" && stringValue != "f");

                if (stringValue == "t")
                {
                    permissions[pair.Key] = true;
                }
                else if (stringValue == "f")
                {
                    permissions[pair.Key] = false;
                }
            }

            return await _roleService.SetUpRole(room, role, permissions);
        }

        private async Task<bool> DeleteRole(Room room, Role role)
        {
            if (await _roleService.DeleteRole(room, role))
            {
                System.Console.WriteLine("Role deleted successfully!");
                return true;
            }

            System.Console.WriteLine("Error! Please, try again later!");

            return false;
        }

        private async Task GetUsersRoles(Room room)
        {
            var rolesOfUsers = await _roleService.GetRolesOfUsers(room.Id);
            foreach (var user in rolesOfUsers)
            {
                System.Console.WriteLine($"{user.Key} - {user.Value}");
            }
        }
    }
}
