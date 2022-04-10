using System.Linq;
using System.Threading.Tasks;
using PL.Console.Interfaces;

namespace PL.Console
{
    public class App
    {
        private readonly IRegistration _registration;
        private readonly IAuthorization _authorization;
        private readonly IRoomsControl _roomsControl;
        private readonly IUserControl _userControl;
        private readonly IResetPasswordControl _resetPasswordControl;


        public App(IAuthorization authorization, IRegistration registration,
            IRoomsControl roomsControl, IUserControl userControl, IResetPasswordControl resetPasswordControl)
        {
            this._roomsControl = roomsControl;
            this._registration = registration;
            this._authorization = authorization;
            this._userControl = userControl;
            this._resetPasswordControl = resetPasswordControl;
        }

        public async Task StartApp()
        {
            while (true)
            {
                var userKeys = new[] {"y", "n", "f"};
                string key;
                do
                {
                    System.Console.WriteLine(
                        "Wanna sign up (press \"y\"), wanna sign in (press \"n\"), forgot password - press \"f\"");
                    key = System.Console.ReadLine()?.Trim();
                } while (string.IsNullOrWhiteSpace(key) || !userKeys.Contains(key));

                if (key == "y")
                {
                    await _registration.RegisterUserAsync();
                }
                else if (key == "n")
                {
                    var response = await _authorization.AuthorizeUserAsync();

                    if (response)
                    {
                        System.Console.WriteLine("Successfully logged in!");
                    }
                    else
                    {
                        System.Console.WriteLine("Login failed, please try again later!");
                    }
                }
                else if (key == "f")
                {
                    var response = await _resetPasswordControl.ResetUserPasswordAsync();

                    if (response)
                    {
                        System.Console.WriteLine("Password reset successfully!");
                    }
                    else
                    {
                        System.Console.WriteLine("Reset password failed, please try again later!");
                        continue;
                    }
                }

                string accountSet;
                do
                {
                    System.Console.WriteLine("Do you want to set up your account? (press \"y\" or \"n\")");
                    accountSet = System.Console.ReadLine();
                } while (accountSet != "y" && accountSet != "n");

                if (accountSet == "y")
                {
                    _userControl.ChooseAction();
                }

                while (true)
                {
                    await _roomsControl.ShowUserRooms();
                }
            }
        }
    }
}
