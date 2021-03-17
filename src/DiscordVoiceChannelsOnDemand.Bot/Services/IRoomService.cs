using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

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
        /// <inheritdoc cref="DeleteRoomAsync(Discord.WebSocket.SocketVoiceChannel)"/>
        /// </summary>
        /// <param name="voiceChannelId"></param>
        /// <param name="guildId"></param>
        /// <returns></returns>
        Task DeleteRoomAsync(ulong voiceChannelId, ulong guildId);
    }
}