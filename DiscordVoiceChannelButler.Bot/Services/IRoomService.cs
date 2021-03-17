using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordVoiceChannelButler.Bot.Services
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
        Task<IVoiceChannel> CreateNewRoomAsync(SocketGuildUser user);

        /// <summary>
        /// Removes a room.
        /// </summary>
        /// <param name="voiceChannel"></param>
        /// <returns></returns>
        Task DeleteRoomAsync(SocketVoiceChannel voiceChannel);
    }
}