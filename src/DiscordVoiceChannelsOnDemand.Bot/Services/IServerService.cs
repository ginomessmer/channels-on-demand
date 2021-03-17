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

        /// <summary>
        /// Checks whether the guild is registered as a server.
        /// </summary>
        /// <param name="guild"></param>
        /// <returns></returns>
        Task<bool> IsRegisteredAsync(IGuild guild);

        /// <summary>
        /// Registers a new server from a guild.
        /// </summary>
        /// <param name="guild"></param>
        /// <returns></returns>
        Task RegisterAsync(IGuild guild);
    }
}