using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure
{
    public interface IGenericRepository<T>
    {
        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task AddAsync(T item);

        /// <summary>
        /// Removes an entity entirely.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveAsync(string id);

        /// <summary>
        /// Checks whether a voice channel exists in the repository. Returns true if that's the case.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string id);

        /// <summary>
        /// Returns all voice channels that are stored.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();
    }
}