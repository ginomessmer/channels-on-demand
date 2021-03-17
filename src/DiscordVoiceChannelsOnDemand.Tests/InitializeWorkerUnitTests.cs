using System.Threading;
using System.Threading.Tasks;
using Discord;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using DiscordVoiceChannelsOnDemand.Bot.Workers;
using Moq;
using Xunit;

namespace DiscordVoiceChannelsOnDemand.Tests
{
    public class InitializeWorkerUnitTests
    {
        [Fact]
        public async Task InitializeWorker_Execute_RegisterServerFromNewGuild()
        {
            // Arrange

            // Mock => Sample Guild
            var guildMock = new Mock<IGuild>();

            // Mock => IServerService
            var serverServiceMock = new Mock<IServerService>();
            serverServiceMock.Setup(x => x.GetAllGuildsAsync()).ReturnsAsync(new[]
            {
                guildMock.Object
            });

            var worker = new InitializeWorker(serverServiceMock.Object);

            // Act
            await worker.StartAsync(CancellationToken.None);

            // Assert
            serverServiceMock.Verify(x => x.RegisterAsync(guildMock.Object));
        }

        [Fact]
        public async Task InitializeWorker_Execute_SkipRegistration()
        {
            // Arrange

            // Mock => Sample Guild
            var guildMock = new Mock<IGuild>();
            var placeboGuildMock = new Mock<IGuild>();

            // Mock => IServerService
            var serverServiceMock = new Mock<IServerService>();
            {
                serverServiceMock.Setup(x => x.GetAllGuildsAsync())
                    .ReturnsAsync(new []{placeboGuildMock.Object});
                serverServiceMock.Setup(x => x.IsRegisteredAsync(It.IsNotIn(guildMock.Object)))
                    .ReturnsAsync(false);
                serverServiceMock.Setup(x => x.IsRegisteredAsync(placeboGuildMock.Object))
                    .ReturnsAsync(true);
            }

            var worker = new InitializeWorker(serverServiceMock.Object);

            // Act
            await worker.StartAsync(CancellationToken.None);

            // Assert
            serverServiceMock.Verify(x => x.RegisterAsync(guildMock.Object), Times.Never);
        }
    }
}