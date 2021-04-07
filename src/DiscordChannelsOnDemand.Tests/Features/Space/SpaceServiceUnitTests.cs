using Discord;
using DiscordChannelsOnDemand.Bot.Models;
using DiscordChannelsOnDemand.Tests.SeedWork;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordChannelsOnDemand.Bot.Features.Spaces;
using Xunit;

namespace DiscordChannelsOnDemand.Tests.Features.Space
{
    public class SpaceServiceUnitTests
    {
        private readonly Mock<IDiscordClient> _discordClientMock = new();
        private readonly Mock<ISpaceRepository> _spaceRepositoryMock = new();
        private readonly Mock<IMessageChannel> _channelMock = new();
        private readonly DateTime _editedTimeStampTwoDaysOffset = DateTime.UtcNow - TimeSpan.FromDays(2);
        private readonly DateTime _editedTimeStampTwoMinutesOffset = DateTime.UtcNow - TimeSpan.FromMinutes(2);

        private readonly TimeSpan _expiry = TimeSpan.FromDays(1);

        private readonly Bot.Models.Space _space = new();
        private readonly Server _server = new();

        public SpaceServiceUnitTests()
        {
            SetupChannelMock(_editedTimeStampTwoDaysOffset);

            SetupServerModel();

            _spaceRepositoryMock
                .Setup(x => 
                    x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(_space);

            _discordClientMock
                .Setup(x =>
                    x.GetChannelAsync(It.IsAny<ulong>(), It.IsAny<CacheMode>(), It.IsAny<RequestOptions>()))
                .ReturnsAsync(_channelMock.Object);
        }

        // Setups
        private void SetupChannelMock(DateTime editedTimeStamp, int count = 1)
        {
            _channelMock
                .Setup(x =>
                    x.GetMessagesAsync(1, It.IsAny<CacheMode>(), It.IsAny<RequestOptions>()))
                .Returns(GetMockChannelsAsync(editedTimeStamp, count));
        }

        private void SetupServerModel()
        {
            _server.SpaceConfiguration = new ServerSpaceConfiguration
            {
                SpaceTimeoutThreshold = _expiry
            };

            _space.Server = _server;
        }

        [Fact]
        public async Task SpaceService_LastActivity_DetermineCorrectly()
        {
            // Arrange
            var spaceService = new SpaceService(_discordClientMock.Object, _spaceRepositoryMock.Object, null);

            // Act
            var result = await spaceService.GetLastActivityAsync(It.IsAny<string>());

            // Assert
            Assert.Equal(_editedTimeStampTwoDaysOffset, result);
        }

        [Theory]
        [MemberData(nameof(SupplyTimeStampTestData))]
        public async Task SpaceService_ShouldRemove_DetermineCorrectly(DateTime editedTimeStamp, bool expectedResult)
        {
            // Arrange
            SetupChannelMock(editedTimeStamp);
            var spaceService = new SpaceService(_discordClientMock.Object, _spaceRepositoryMock.Object, null);

            // Act
            var result = await spaceService.ShouldRemoveSpaceAsync(It.IsAny<string>());

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [MemberData(nameof(SupplyTimeStampTestDataForEmptyChannel))]
        public async Task SpaceService_ShouldRemoveEmptyChannel_DetermineCorrectly(DateTime createdAt, bool expectedResult)
        {
            // Arrange
            SetupChannelMock(_editedTimeStampTwoMinutesOffset, 0);
            _channelMock.Setup(x => x.CreatedAt).Returns(createdAt);

            var spaceService = new SpaceService(_discordClientMock.Object, _spaceRepositoryMock.Object, null);

            // Act
            var result = await spaceService.ShouldRemoveSpaceAsync(It.IsAny<string>());

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Test()
        {
            // Arrange
            var userFake = new GuildUserFake();
            var host = userFake.Generate();
            var users = userFake.Generate(10).Cast<IGuildUser>().ToList();
            
            var channelMock = new Mock<IGuildChannel>();
            channelMock.Setup(x => x.PermissionOverwrites).Returns(users
                .Take(3)
                .Select(x => 
                    new Overwrite(x.Id, PermissionTarget.User, new OverwritePermissions(viewChannel: PermValue.Allow)))
                .ToList);

            channelMock.SetupGet(x => x.Guild).Returns(new GuildFake());

            var spaceService = new SpaceService(_discordClientMock.Object, _spaceRepositoryMock.Object, null);

            // Act
            await spaceService.ApplyPermissionsAsync(channelMock.Object, host, users.ToArray());

            // Assert

        }

        // Helper methods
        public static IEnumerable<object[]> SupplyTimeStampTestData => new[]
        {
            new object[] { DateTime.UtcNow - TimeSpan.FromHours(2), false },
            new object[] { DateTime.UtcNow - TimeSpan.FromMinutes(10), false },
            new object[] { DateTime.UtcNow - TimeSpan.FromDays(2), true },
            new object[] { DateTime.UtcNow - TimeSpan.FromDays(10), true }
        };

        public static IEnumerable<object[]> SupplyTimeStampTestDataForEmptyChannel = new[]
        {
            new object[] {DateTime.UtcNow - TimeSpan.FromMinutes(2), false},
            new object[] {DateTime.UtcNow - TimeSpan.FromDays(2), true}
        };

        public static async IAsyncEnumerable<IReadOnlyCollection<IMessage>> GetMockChannelsAsync(DateTime editedTimeStamp, int count = 1)
        {
            var mock = new Mock<IMessage>();
            mock.Setup(x => x.EditedTimestamp).Returns(editedTimeStamp);

            var enumerable = Enumerable.Repeat(mock.Object, count).ToList();
            yield return await Task.FromResult((IReadOnlyCollection<IMessage>)enumerable);
        }
    }
}