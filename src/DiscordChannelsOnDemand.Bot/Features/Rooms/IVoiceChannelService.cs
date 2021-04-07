using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;

namespace DiscordChannelsOnDemand.Bot.Features.Rooms
{
    public interface IVoiceChannelService
    {
        Task<IEnumerable<IVoiceChannel>> GetAllAsync();

        Task<IEnumerable<IVoiceChannel>> GetAllAsync(ulong guildId);
    }
}