using DiscordVoiceChannelsOnDemand.Bot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure
{
    /// <summary>
    /// The room repository - what did ya expect?
    /// </summary>
    public interface IRoomRepository
    {
        /// <summary>
        /// Adds a new room to the repository.
        /// </summary>
        /// <param name="voiceChannelId">The voice channel's ID</param>
        /// <param name="hostUserId">The host's user ID</param>
        /// <returns>The persisted room model instance</returns>
        Task<Room> AddAsync(string voiceChannelId, string hostUserId);

        /// <summary>
        /// Removes a room entirely.
        /// </summary>
        /// <param name="voiceChannelId"></param>
        /// <returns></returns>
        Task RemoveAsync(string voiceChannelId);

        /// <summary>
        /// Checks whether a voice channel exists in the repository. Returns true if that's the case.
        /// </summary>
        /// <param name="voiceChannelId"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string voiceChannelId);

        /// <summary>
        /// Returns all voice channels that are stored.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Room>> GetAllAsync();
    }
}
