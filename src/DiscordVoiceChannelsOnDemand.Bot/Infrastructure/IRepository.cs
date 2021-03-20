using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Adds a new item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task AddAsync(T item);

        /// <summary>
        /// Removes an item entirely.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveAsync(string id);

        /// <summary>
        /// Checks whether an item exists in the repository. Returns true if that's the case.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task UpdateAsync(T item);

        /// <summary>
        /// Returns all items stored in the repository.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Gets a single item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetAsync(string id);

        /// <summary>
        /// Queries the repository by expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<IQueryable<T>> QueryAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Commits all changes to the data store.
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}