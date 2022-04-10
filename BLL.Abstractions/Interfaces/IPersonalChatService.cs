using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace BLL.Abstractions.Interfaces
{
    public interface IPersonalChatService
    {
        Task CreatePersonalChat(List<string> usersToChatWith);
        
        Task<bool> ChangeNameOfPersonalChat(PersonalChat chat, string name);
        
        Task AddParticipantsToPersonalChat(PersonalChat chat, List<string> participants);
        
        Task<IList<PersonalChat>> GetUserPersonalChats();
        
        Task<bool> LeavePersonalChat(PersonalChat chat);
        
        Task<IList<string>> GetUserNamesOfChat(PersonalChat chat);
    }
}
