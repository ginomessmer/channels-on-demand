using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;

namespace DiscordVoiceChannelsOnDemand.Bot.Services
{
    public interface IServerService
    {
        Task<IEnumerable<IGuild>> GetAllGuildsAsync();
    }
}