using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordChannelsOnDemand.Bot.Features.Servers;
using DiscordChannelsOnDemand.Bot.Features.Spaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordChannelsOnDemand.Bot.Features.Rooms
{
    /// <summary>
    /// This worker creates a new room on demand whenever someone joins
    /// the lobby voice channel.
    /// </summary>
    public class CreateRoomWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DiscordSocketClient _client;
        private readonly ILogger<CreateRoomWorker> _logger;

        public CreateRoomWorker(IServiceProvider serviceProvider,
            DiscordSocketClient client,
            ILogger<CreateRoomWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _client = client;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _client.UserVoiceStateUpdated += ClientOnUserVoiceStateUpdated;
            return Task.CompletedTask;
        }

        private Task ClientOnUserVoiceStateUpdated(SocketUser socketUser,
            SocketVoiceState previousState, SocketVoiceState newState)
        {
            var voiceChannel = newState.VoiceChannel ?? previousState.VoiceChannel;
            return HandleAsync(socketUser, voiceChannel);
        }

        private async Task HandleAsync(IUser socketUser, IVoiceChannel voiceChannel)
        {
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = new
            {
                ServerService = scope.ServiceProvider.GetRequiredService<IServerService>(),
                RoomService = scope.ServiceProvider.GetRequiredService<IRoomService>(),
                SpaceService = scope.ServiceProvider.GetRequiredService<ISpaceService>()
            };


            // Check if user is typeof(SocketGuildUser)
            if (socketUser is not SocketGuildUser user)
                return;

            if (voiceChannel is null)
                return;

            // Check whether lobby exists for the channel
            if (!await unitOfWork.ServerService.IsLobbyRegisteredAsync(voiceChannel))
                return;

            // Create new voice channel
            var lobby = await unitOfWork.ServerService.GetLobbyAsync(voiceChannel);
            var channel = await unitOfWork.RoomService.CreateNewRoomAsync(user, lobby);

            // Create space if auto-create is enabled
            if (lobby.AutoCreateSpace)
            {
                var space = await unitOfWork.SpaceService.CreateSpaceAsync(user);
                await unitOfWork.RoomService.LinkSpaceAsync(lobby.TriggerVoiceChannelId, space.TextChannelId);
            }

            _logger.LogInformation("Created new room {VoiceChannel} for user {User}", channel, user);
        }
    }
}
