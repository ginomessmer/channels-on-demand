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
    }
}