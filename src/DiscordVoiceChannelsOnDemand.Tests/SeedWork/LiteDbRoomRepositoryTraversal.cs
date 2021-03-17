using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Models;
using LiteDB;
using System.Collections.Generic;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure.LiteDb;

namespace DiscordVoiceChannelsOnDemand.Tests.SeedWork
{
    public class LiteDbRoomRepositoryTraversal : LiteDbRoomRepository
    {
        /// <inheritdoc />
        public LiteDbRoomRepositoryTraversal(LiteDatabase database) : base(database)
        {
        }

        public IEnumerable<Room> GetAll() => Collection.FindAll();
    }
}