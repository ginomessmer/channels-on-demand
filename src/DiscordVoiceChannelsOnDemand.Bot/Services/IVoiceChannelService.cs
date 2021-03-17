using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;

namespace DiscordVoiceChannelsOnDemand.Bot.Services
{
    public interface IVoiceChannelService
    {
        Task<IEnumerable<IVoiceChannel>> GetAllAsync();

        Task<IEnumerable<IVoiceChannel>> GetAllAsync(ulong guildId);
    }
}