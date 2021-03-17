using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordVoiceChannelsOnDemand.Bot.Models;
using LiteDB;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure
{
    public class LiteDbTenantRepository : LiteDbRepositoryBase<Tenant>, ITenantRepository
    {
        /// <inheritdoc />
        public LiteDbTenantRepository(ILiteDatabase database) : base(database)
        {
        }

        #region Overrides of LiteDbRepositoryBase<Tenant>

        /// <inheritdoc />
        public override Task AddAsync(Tenant item)
        {
            Collection.EnsureIndex(x => x.GuildId);
            return base.AddAsync(item);
        }

        #endregion

        #region Implementation of ITenantRepository

        /// <inheritdoc />
        public Task<IEnumerable<Slot>> QueryAllSlotsAsync()
        {
            return Task.FromResult(Collection.FindAll().SelectMany(x => x.Slots));
        }

        #endregion
    }
}