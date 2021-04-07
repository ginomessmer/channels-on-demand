using DiscordChannelsOnDemand.Bot.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordChannelsOnDemand.Bot.Core.Infrastructure.EntityFramework
{
    public class EfCoreRoomRepository : EfCoreRepository<Room>, IRoomRepository
    {
        /// <inheritdoc />
        public EfCoreRoomRepository(BotDbContext botDbContext) : base(botDbContext)
        {
        }
        
        /// <inheritdoc />
        public async Task<Room> AddAsync(string voiceChannelId, string hostUserId, string guildId)
        {
            var room = new Room(voiceChannelId, hostUserId, guildId);
            await Set.AddAsync(room);
            return room;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Room>> GetAllAsync(string guildId)
        {
            var rooms =  await Set.AsQueryable().Where(x => x.GuildId == guildId).ToListAsync();
            return rooms;
        }

        #region Overrides of EfCoreRepository<Room>

        /// <inheritdoc />
        public override Task<Room> GetAsync(string id) => 
            Set.Include(x => x.LinkedSpace)
                .SingleOrDefaultAsync(x => x.ChannelId == id);

        #endregion
    }
}