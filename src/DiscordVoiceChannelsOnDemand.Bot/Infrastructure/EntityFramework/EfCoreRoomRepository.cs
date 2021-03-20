using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordVoiceChannelsOnDemand.Bot.Models;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure.EntityFramework
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
        public Task<IEnumerable<Room>> GetAllAsync(string guildId)
        {
            throw new NotImplementedException();
        }
    }
}