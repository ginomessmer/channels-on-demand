using System.Threading.Tasks;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DiscordVoiceChannelsOnDemand.Tests
{
    public class EfCoreIntegrationTests
    {
        private readonly BotDbContext _botDbContext;
        private readonly IRoomRepository _roomRepository;

        public EfCoreIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<BotDbContext>().UseInMemoryDatabase("Test").Options;
            _botDbContext = new BotDbContext(options);
            _roomRepository = new EfCoreRoomRepository(_botDbContext);
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