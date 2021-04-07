﻿using Discord;
using DiscordChannelsOnDemand.Bot.Core.Extensions;
using Moq;
using Xunit;

namespace DiscordChannelsOnDemand.Tests
{
    public class ExtensionTests
    {
        [Theory]
        [InlineData("Martha", "", "Martha")]
        [InlineData("", "MarthaDaBest", "MarthaDaBest")]
        [InlineData("Martha", "MarthaDaBest", "MarthaDaBest")]
        public void GuildUser_PreferredName_DetermineCorrectly(string username, string nickname, string expected)
        {
            // Arrange
            var mock = new Mock<IGuildUser>();
            mock.SetupGet(x => x.Username).Returns(username);
            mock.SetupGet(x => x.Nickname).Returns(nickname);

            // Act
            var name = mock.Object.GetPreferredName();

            // Assert
            Assert.Equal(expected, name);
        }
    }
}
