using BLL.Abstractions.Interfaces;
using Core;

namespace BLL.Helpers
{
    public class CurrentUserConsole : ICurrentUser
    {
        public User User { get; set; }
    }
}
