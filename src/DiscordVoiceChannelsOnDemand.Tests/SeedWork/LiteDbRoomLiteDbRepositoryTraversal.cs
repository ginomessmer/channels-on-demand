using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Models;
using LiteDB;
using System.Collections.Generic;

namespace DiscordVoiceChannelsOnDemand.Tests.SeedWork
{
    public class LiteDbRoomLiteDbRepositoryTraversal : LiteDbRoomLiteDbRepository
    {
        /// <inheritdoc />
        public LiteDbRoomLiteDbRepositoryTraversal(LiteDatabase database) : base(database)
        {
        }

        public IEnumerable<Room> GetAll() => Collection.FindAll();
    }
}