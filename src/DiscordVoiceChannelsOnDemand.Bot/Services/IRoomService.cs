using System.Collections.Generic;
using Discord;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace DiscordVoiceChannelsOnDemand.Bot.Services
{
    /// <summary>
    /// A service class that is responsible for managing the rooms.
    /// </summary>
    public interface IRoomService
    {
        /// <summary>
        /// Creates a new room and persists it in the state.
        /// </summary>
        /// <param name="user">The owner of the room.</param>
        /// <returns>The voice channel room.</returns>
        Task<IVoiceChannel> CreateNewRoomAsync(IGuildUser user);

        /// <summary>
        /// Removes a room.
        /// </summary>
        /// <param name="voiceChannel"></param>
        /// <returns></returns>
        Task DeleteRoomAsync(IVoiceChannel voiceChannel);

        /// <summary>
        /// <inheritdoc cref="DeleteRoomAsync(Discord.IVoiceChannel)"/>
        /// </summary>
        /// <param name="voiceChannelId"></param>
        /// <param name="guildId"></param>
        /// <returns></returns>
        Task DeleteRoomAsync(ulong voiceChannelId, ulong guildId);

        /// <summary>
        /// Gets all rooms as voice channels.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IVoiceChannel>> GetAllRoomVoiceChannelsAsync();

        /// <summary>
        /// Checks if the <paramref name="voiceChannel"/> is registered as a room.
        /// </summary>
        /// <param name="voiceChannel"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(IVoiceChannel voiceChannel);
    }
}