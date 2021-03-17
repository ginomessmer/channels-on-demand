using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordVoiceChannelsOnDemand.Bot.Models;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure
{
    public interface ITenantRepository : IGenericRepository<Tenant>
    {
        Task<IEnumerable<Slot>> QueryAllSlotsAsync();
    }
}