using DiscordVoiceChannelsOnDemand.Bot.Models;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure.EntityFramework
{
    public class EfCoreSpaceRepository : EfCoreRepository<Space>, ISpaceRepository
    {
        /// <inheritdoc />
        public EfCoreSpaceRepository(BotDbContext botDbContext) : base(botDbContext)
        {
        }
    }
}