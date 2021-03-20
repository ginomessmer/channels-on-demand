using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure.EntityFramework
{
    public abstract class EfCoreRepository<T> : IRepository<T> where T : class
    {
        private readonly BotDbContext _botDbContext;

        protected EfCoreRepository(BotDbContext botDbContext)
        {
            _botDbContext = botDbContext;
        }

        #region Implementation of IRepository<T>

        /// <inheritdoc />
        public Task AddAsync(T item) => Set.AddAsync(item).AsTask();

        /// <inheritdoc />
        public async Task RemoveAsync(string id)
        {
            var result = await Set.FindAsync(id);
            Set.Remove(result);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(string id)
        {
            var entity = await Set.FindAsync(id);
            return entity is not null;
        }

        /// <inheritdoc />
        public Task UpdateAsync(T item) => Task.FromResult(Set.Update(item));

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = await Set.AsQueryable().ToListAsync();
            return result;
        }

        /// <inheritdoc />
        public Task<T> GetAsync(string id) => Set.FindAsync(id).AsTask();

        /// <inheritdoc />
        public Task<IQueryable<T>> QueryAsync(Expression<Func<T, bool>> expression) => Task.FromResult(Set.AsQueryable());

        /// <inheritdoc />
        public Task SaveChangesAsync() => _botDbContext.SaveChangesAsync();

        #endregion

        protected DbSet<T> Set => _botDbContext.Set<T>();
    }
}