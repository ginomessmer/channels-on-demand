using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiteDB;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure
{
    public abstract class LiteDbRepositoryBase<T> where T : class
    {
        private readonly ILiteDatabase _database;

        protected LiteDbRepositoryBase(ILiteDatabase database)
        {
            _database = database;
        }

        protected ILiteCollection<T> Collection => _database.GetCollection<T>();

        /// <inheritdoc />
        public virtual Task AddAsync(T item) => Task.FromResult(Collection.Insert(item));

        /// <inheritdoc />
        public Task RemoveAsync(string id) => Task.FromResult<bool>(Collection.Delete(id));

        /// <inheritdoc />
        public Task<bool> ExistsAsync(string id) => Task.FromResult(Collection.FindById(id) is not null);

        /// <inheritdoc />
        public Task<IEnumerable<T>> GetAllAsync() => Task.FromResult<IEnumerable<T>>(Collection.FindAll());

        /// <inheritdoc />
        public Task<T> GetAsync(string id) => Task.FromResult(Collection.FindById(id));

        public Task UpdateAsync(T item) => Task.FromResult(Collection.Update(item));

        public Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> expression) => Task.FromResult(Collection.Find(expression));
    }
}