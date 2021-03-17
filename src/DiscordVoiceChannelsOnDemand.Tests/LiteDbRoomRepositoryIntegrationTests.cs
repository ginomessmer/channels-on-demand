using DiscordVoiceChannelsOnDemand.Tests.SeedWork;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DiscordVoiceChannelsOnDemand.Tests
{
    public class LiteDbRoomRepositoryIntegrationTests : IClassFixture<LiteDbFactory>
    {
        private readonly LiteDbFactory _dbFactory;

        public LiteDbRoomRepositoryIntegrationTests(LiteDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        [Fact]
        public async Task LiteDbRoomRepository_Add_ExistSuccessfully()
        {
            // Arrange
            var repository = new LiteDbRoomRepositoryTraversal(_dbFactory.CreateDatabase());
            
            // Act
            var room = await repository.AddAsync("1111", "2222", "3333");

            // Assert
            var rooms = repository.GetAll().ToList();
            Assert.NotNull(room);
            Assert.Single(rooms);
            Assert.Contains(rooms, r => r.ChannelId == room.ChannelId);
        }

        [Fact]
        public async Task LiteDbRoomRepository_AddRemove_Successfully()
        {
            // Arrange
            var repository = new LiteDbRoomRepositoryTraversal(_dbFactory.CreateDatabase());

            // Act
            var room = await repository.AddAsync("1111", "2222", "3333");
            await repository.RemoveAsync(room.ChannelId);

            // Assert
            Assert.Empty(repository.GetAll());
        }

        [Fact]
        public async Task LiteDbRoomRepository_FindAll_Successfully()
        {
            // Arrange
            var repository = new LiteDbRoomRepositoryTraversal(_dbFactory.CreateDatabase());

            // Act
            var room1 = await repository.AddAsync("11111", "2222", "3333");
            var room2 = await repository.AddAsync("11112", "2222", "3333");
            var room3 = await repository.AddAsync("11113", "2222", "33331");


            var rooms = (await repository.GetAllAsync()).ToList();

            // Assert
            Assert.Equal(3, rooms.Count);
            Assert.Collection(rooms,
                x => Assert.Equal(room1.ChannelId, x.ChannelId),
                x => Assert.Equal(room2.ChannelId, x.ChannelId),
                x => Assert.Equal(room3.ChannelId, x.ChannelId));
        }

        [Fact]
        public async Task LiteDbRoomRepository_FindByGuild_Successfully()
        {
            // Arrange
            var repository = new LiteDbRoomRepositoryTraversal(_dbFactory.CreateDatabase());

            // Act
            var room1 = await repository.AddAsync("11111", "2222", "3333");
            var room2 = await repository.AddAsync("11112", "2222", "3333");
            var room3 = await repository.AddAsync("11113", "2222", "33331");


            var rooms = (await repository.GetAllAsync("3333")).ToList();

            // Assert
            Assert.Equal(2, rooms.Count());
            Assert.All(rooms, x => Assert.Equal("3333", x.GuildId));
        }
    }
}
