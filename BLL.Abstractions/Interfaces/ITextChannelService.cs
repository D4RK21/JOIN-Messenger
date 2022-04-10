using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace BLL.Abstractions.Interfaces
{
    public interface ITextChannelService
    {
        Task<bool> EditTextChannel(TextChannel textChannel, Room room, string name = null, string description = null, bool? isAdmin = false);

        Task<bool> DeleteTextChannel(TextChannel textChannel, Room room);
        
        Task<bool> CreateTextChannel(Room room, string name, string description, bool isAdmin);

        Task<List<TextChannel>> GetTextChannels(Room room);

        Task<bool> CanUseAdminChannels(Room room, User user=null);

        Task<bool> CanManageChannels(Room room, User user=null);
    }
}
