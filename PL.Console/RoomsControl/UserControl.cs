using BLL.Abstractions.Interfaces;
using PL.Console.Interfaces;

namespace PL.Console.RoomsControl
{
    public class UserControl : IUserControl
    {
        private readonly IPasswordService _passwordService;
        private readonly IUserService _userService;

        public UserControl(IPasswordService passwordService, IUserService userService)
        {
            _passwordService = passwordService;
            _userService = userService;
        }

        public bool ChooseAction()
        {
            string action;
            do
            {
                System.Console.Write("What do you want to change (\"password\" or \"name\")? ");
                action = System.Console.ReadLine();
            } while (action != "password" && action != "name");

            if (action == "password")
            {
                return ChangePassword();
            }

            if (action == "name")
            {
                ChangeNames();
                return true;
            }

            return false;
        }

        private bool ChangePassword()
        {
            string oldPass;
            do
            {
                System.Console.Write("Enter your old password: ");
                oldPass = System.Console.ReadLine()?.Trim();
            } while (string.IsNullOrWhiteSpace(oldPass));

            string newPass;
            do
            {
                System.Console.Write("Enter your new password: ");
                newPass = System.Console.ReadLine()?.Trim();
            } while (string.IsNullOrWhiteSpace(newPass));

            return _passwordService.ChangePassword(oldPass, newPass).Result;
        }

        private void ChangeNames()
        {
            string firstNameChanging;
            do
            {
                System.Console.Write("Do you want to change your first name (\"y\" or \"n\"): ");
                firstNameChanging = System.Console.ReadLine()?.Trim();
            } while (firstNameChanging != "y" && firstNameChanging != "n");

            string lastNameChanging;
            do
            {
                System.Console.Write("Do you want to change your last name (\"y\" or \"n\"): ");
                lastNameChanging = System.Console.ReadLine()?.Trim();
            } while (lastNameChanging != "y" && lastNameChanging != "n");

            string firstName = null;
            string lastName = null;

            if (firstNameChanging != "n")
            {
                do
                {
                    System.Console.Write("Enter your new first name: ");
                    firstName = System.Console.ReadLine()?.Trim();
                } while (string.IsNullOrWhiteSpace(firstName));
            }

            if (lastNameChanging != "n")
            {
                do
                {
                    System.Console.Write("Enter your new last name: ");
                    lastName = System.Console.ReadLine()?.Trim();
                } while (string.IsNullOrWhiteSpace(lastName));
            }

            _userService.ChangeUserNames(firstName, lastName);
        }
    }
}
