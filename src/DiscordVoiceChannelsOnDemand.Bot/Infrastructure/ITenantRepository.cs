using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordVoiceChannelsOnDemand.Bot.Models;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure
{
    public interface ITenantRepository : IGenericRepository<Tenant>
    {
        Task<IEnumerable<Slot>> QueryAllSlotsAsync();

        Task<bool> SlotsExistsAsync(string voiceChannelId);
        
        Task<Slot> FindSlotAsync(string voiceChannelId);

        Task DeleteSlotAsync(string voiceChannelId);
    }
}