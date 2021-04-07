using DiscordChannelsOnDemand.Bot.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DiscordChannelsOnDemand.Bot.Core.Infrastructure.EntityFramework
{
    public class EfCoreSpaceRepository : EfCoreRepository<Space>, ISpaceRepository
    {
        /// <inheritdoc />
        public EfCoreSpaceRepository(BotDbContext botDbContext) : base(botDbContext)
        {
        }

        #region Overrides of EfCoreRepository<Space>

        /// <inheritdoc />
        public override Task<Space> GetAsync(string id) => Set
            .Include(x => x.Server)
            .FirstOrDefaultAsync(x => x.TextChannelId == id);

        #endregion
    }
}