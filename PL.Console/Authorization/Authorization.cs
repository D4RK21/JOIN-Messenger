using System.Threading.Tasks;
using BLL.Abstractions.Interfaces;
using Core;
using PL.Console.Interfaces;

namespace PL.Console.Authorization
{
    public class Authorization : IAuthorization
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IMailWorker _mailWorker;
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;

        public Authorization(IUserService userService, IAuthorizationService authorizationService,
            IMailWorker mailWorker, ICurrentUser currentUser)
        {
            _userService = userService;
            _authorizationService = authorizationService;
            _mailWorker = mailWorker;
            _currentUser = currentUser;
        }

        public async Task<bool> AuthorizeUserAsync()
        {
            var tempUser = await InputEmailAndPassword();

            if (await _authorizationService.IsLastAuthWasLongAgo(tempUser, 10) ||
                !_userService.IsUserVerified(tempUser))
            {
                var email = tempUser.Email;
                await _mailWorker.SendCodeByEmailAsync(email);

                System.Console.Write($"Enter code from message sent on your email ({email}): ");
                var codeFromUser = System.Console.ReadLine()?.Trim();

                while (!_mailWorker.CompareCodes(codeFromUser))
                {
                    System.Console.Write(
                        "Wrong code! Enter code from message sent on your email or \"r\" to resend code: ");
                    codeFromUser = System.Console.ReadLine()?.Trim();

                    if (codeFromUser == "r")
                    {
                        System.Console.Write($"Enter code from message sent on your email ({email}): ");
                        await _mailWorker.SendCodeByEmailAsync(email);
                        codeFromUser = System.Console.ReadLine()?.Trim();
                    }
                }
            }

            _currentUser.User = tempUser;
            await _authorizationService.UpdateLastAuth(tempUser);
            
            return true;
        }

        private async Task<User> InputEmailAndPassword()
        {
            System.Console.Write("Enter your username/email: ");
            var usernameOrEmail = System.Console.ReadLine()?.Trim();

            System.Console.Write("Enter your password: ");
            var password = System.Console.ReadLine()?.Trim();

            var isUserDataValid = await _authorizationService.CheckUserDataForAuthAsync(usernameOrEmail, password);

            while (!isUserDataValid)
            {
                System.Console.WriteLine("Invalid username or password");

                System.Console.Write("Enter your username/email: ");
                usernameOrEmail = System.Console.ReadLine()?.Trim();

                System.Console.Write("Enter your password: ");
                password = System.Console.ReadLine()?.Trim();

                isUserDataValid = await _authorizationService.CheckUserDataForAuthAsync(usernameOrEmail, password);
            }

            return await _authorizationService.GetInfoAboutUser(usernameOrEmail);
        }
    }
}
