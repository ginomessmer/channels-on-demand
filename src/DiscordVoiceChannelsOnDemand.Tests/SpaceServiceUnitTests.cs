using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Models;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Moq;
using Xunit;

namespace DiscordVoiceChannelsOnDemand.Tests
{
    public class SpaceServiceUnitTests
    {
        private readonly Mock<IDiscordClient> _discordClientMock = new();
        private readonly Mock<ISpaceRepository> _spaceRepositoryMock = new();
        private readonly Mock<IMessageChannel> _channelMock = new();
        private readonly DateTime _editedTimeStamp = DateTime.UtcNow - TimeSpan.FromDays(2);
        private readonly Space _space = new();

        public SpaceServiceUnitTests()
        {
            _channelMock
                .Setup(x => 
                    x.GetMessagesAsync(1, It.IsAny<CacheMode>(), It.IsAny<RequestOptions>()))
                .Returns(GetMockChannelsAsync(_editedTimeStamp));

            _spaceRepositoryMock
                .Setup(x => 
                    x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(_space);

            _discordClientMock
                .Setup(x =>
                    x.GetChannelAsync(It.IsAny<ulong>(), It.IsAny<CacheMode>(), It.IsAny<RequestOptions>()))
                .ReturnsAsync(_channelMock.Object);
        }

        [Fact]
        public async Task SpaceService_LastActivity_DetermineCorrectly()
        {
            // Arrange
            var spaceService = new SpaceService(_discordClientMock.Object, _spaceRepositoryMock.Object);

            // Act
            var result = await spaceService.GetLastActivityAsync(It.IsAny<string>());

            // Assert
            Assert.Equal(_editedTimeStamp, result);
        }

        [Fact]
        public async Task SpaceService_ShouldRemove_DetermineCorrectly()
        {
            // Arrange
            var spaceService = new SpaceService(_discordClientMock.Object, _spaceRepositoryMock.Object);

            // Act
            var result = await spaceService.ShouldRemoveSpaceAsync(It.IsAny<string>());

            // Assert
            Assert.True(result);
        }

        public static async IAsyncEnumerable<IReadOnlyCollection<IMessage>> GetMockChannelsAsync(DateTime editedTimeStamp)
        {
            var mock = new Mock<IMessage>();
            mock.Setup(x => x.EditedTimestamp).Returns(editedTimeStamp);

            yield return await Task.FromResult((IReadOnlyCollection<IMessage>)new List<IMessage>
            {
                mock.Object
            });
        }
    }
}