using Discord;
using Discord.WebSocket;
using DiscordChannelsOnDemand.Bot.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordChannelsOnDemand.Bot.Workers
{
    public class SpaceLobbySyncWorker : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _serviceProvider;

        public SpaceLobbySyncWorker(DiscordSocketClient client,
            IServiceProvider serviceProvider)
        {
            _client = client;
            _serviceProvider = serviceProvider;
        }

        #region Overrides of BackgroundService

        /// <inheritdoc />
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _client.UserVoiceStateUpdated += ClientOnUserVoiceStateUpdated;
            return Task.CompletedTask;
        }

        private async Task ClientOnUserVoiceStateUpdated(SocketUser user, SocketVoiceState previousState, SocketVoiceState newState)
        {
            if (user is not IGuildUser guildUser)
                return;

            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = new
            {
                ServerService = scope.ServiceProvider.GetRequiredService<IServerService>(),
                RoomService = scope.ServiceProvider.GetRequiredService<IRoomService>(),
                SpaceService = scope.ServiceProvider.GetRequiredService<ISpaceService>()
            };

            var voiceChannel = newState.VoiceChannel;
            if (voiceChannel is null)
                return;

            // Get room
            var room = await unitOfWork.RoomService.GetRoomAsync(voiceChannel);
            if (room is null)
                return;

            // Check if it has space
            if (!room.HasSpace)
                return;

            // Add permission
            var space = room.LinkedSpace;
            await unitOfWork.SpaceService.InviteAsync(space.TextChannelId, guildUser);
        }

        #endregion
    }
}