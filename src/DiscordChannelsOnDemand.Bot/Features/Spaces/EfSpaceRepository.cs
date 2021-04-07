using System.Threading.Tasks;
using DiscordChannelsOnDemand.Bot.Core.Infrastructure;
using DiscordChannelsOnDemand.Bot.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordChannelsOnDemand.Bot.Features.Spaces
{
    public class EfSpaceRepository : EfRepository<Space>, ISpaceRepository
    {
        /// <inheritdoc />
        public EfSpaceRepository(BotDbContext botDbContext) : base(botDbContext)
        {
        }

        #region Overrides of EfRepository<Space>

        /// <inheritdoc />
        public override Task<Space> GetAsync(string id) => Set
            .Include(x => x.Server)
            .FirstOrDefaultAsync(x => x.TextChannelId == id);

        #endregion
    }
}