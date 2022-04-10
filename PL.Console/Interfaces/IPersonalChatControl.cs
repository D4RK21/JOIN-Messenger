using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace PL.Console.Interfaces
{
    public interface IPersonalChatControl
    {
        void StartChat();
        
        Task<IList<PersonalChat>> GetUserPersonalChats();
        
        void DoAction();
    }
}
