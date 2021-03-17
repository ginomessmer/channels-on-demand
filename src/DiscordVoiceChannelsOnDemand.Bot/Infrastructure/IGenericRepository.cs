using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordVoiceChannelsOnDemand.Bot.Models;

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
        /// 
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        Task UpdateAsync(T slot);

        /// <summary>
        /// Returns all voice channels that are stored.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Gets a single item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetAsync(string id);
    }
}