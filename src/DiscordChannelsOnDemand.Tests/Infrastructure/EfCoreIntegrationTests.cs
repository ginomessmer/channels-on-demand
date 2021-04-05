using System.Threading.Tasks;
using DiscordChannelsOnDemand.Bot.Infrastructure;
using DiscordChannelsOnDemand.Bot.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DiscordChannelsOnDemand.Tests.Infrastructure
{
    public class EfCoreIntegrationTests
    {
        private readonly IRoomRepository _roomRepository;

        public EfCoreIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<BotDbContext>().UseInMemoryDatabase("Test").Options;
            var botDbContext = new BotDbContext(options);
            _roomRepository = new EfCoreRoomRepository(botDbContext);
        }

        [Fact]
        public async Task Test()
        {
            // Arrange

            // Act
            var addedRoom = await _roomRepository.AddAsync("test", "", "");
            await _roomRepository.SaveChangesAsync();

            // Assert
            var room = _roomRepository.GetAsync("test");
        }
    }
}