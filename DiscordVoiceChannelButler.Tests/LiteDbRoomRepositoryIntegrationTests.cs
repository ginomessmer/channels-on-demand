using System.Linq;
using System.Threading.Tasks;
using DiscordVoiceChannelButler.Tests.SeedWork;
using Xunit;

namespace DiscordVoiceChannelButler.Tests
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
            var room = await repository.AddAsync("1234", "5678");

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
            var room = await repository.AddAsync("1234", "5678");
            await repository.RemoveAsync(room.ChannelId);

            // Assert
            Assert.Empty(repository.GetAll());
        }
    }
}
