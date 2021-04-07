using DiscordChannelsOnDemand.Bot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordChannelsOnDemand.Bot.Core.Infrastructure
{
    /// <summary>
    /// The room repository - what did ya expect?
    /// </summary>
    public interface IRoomRepository : IRepository<Room>
    {
        /// <summary>
        /// Adds a new room to the repository.
        /// </summary>
        /// <param name="voiceChannelId">The voice channel's ID</param>
        /// <param name="hostUserId">The host's user ID</param>
        /// <param name="guildId"></param>
        /// <returns>The persisted room model instance</returns>
        Task<Room> AddAsync(string voiceChannelId, string hostUserId, string guildId);

        /// <summary>
        /// <inheritdoc cref="GetAllAsync()"/>
        /// </summary>
        /// <param name="guildId">The guild's ID that is used to filter the results</param>
        /// <returns></returns>
        Task<IEnumerable<Room>> GetAllAsync(string guildId);
    }
}
