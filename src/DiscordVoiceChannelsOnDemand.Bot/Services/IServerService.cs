using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;

namespace DiscordVoiceChannelsOnDemand.Bot.Services
{
    public interface IServerService
    {
        Task<IEnumerable<IGuild>> GetAllGuildsAsync();

        /// <summary>
        /// Checks whether a lobby is registered that is linked to <paramref name="voiceChannel"/>.
        /// </summary>
        /// <param name="voiceChannel"></param>
        /// <returns></returns>
        Task<bool> IsLobbyRegisteredAsync(IVoiceChannel voiceChannel);
    }
}