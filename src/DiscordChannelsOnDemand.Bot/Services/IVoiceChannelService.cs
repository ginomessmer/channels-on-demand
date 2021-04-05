using Discord;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordChannelsOnDemand.Bot.Services
{
    public interface IVoiceChannelService
    {
        Task<IEnumerable<IVoiceChannel>> GetAllAsync();

        Task<IEnumerable<IVoiceChannel>> GetAllAsync(ulong guildId);
    }
}