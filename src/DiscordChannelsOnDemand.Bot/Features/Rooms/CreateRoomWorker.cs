using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordChannelsOnDemand.Bot.Events;
using DiscordChannelsOnDemand.Bot.Features.Servers;
using DiscordChannelsOnDemand.Bot.Features.Spaces;
using MediatR;
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
        private readonly IMediator _mediator;

        public CreateRoomWorker(IServiceProvider serviceProvider,
            DiscordSocketClient client,
            ILogger<CreateRoomWorker> logger,
            IMediator mediator)
        {
            _serviceProvider = serviceProvider;
            _client = client;
            _logger = logger;
            _mediator = mediator;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _client.UserVoiceStateUpdated += ClientOnUserVoiceStateUpdated;
            return Task.CompletedTask;
        }

        private async Task ClientOnUserVoiceStateUpdated(SocketUser socketUser,
            SocketVoiceState previousState, SocketVoiceState newState)
        {
            var hasJoinedNewChannel = newState.VoiceChannel is not null;
            var hasParticipatedInPreviousChannel = previousState.VoiceChannel is not null;
            var hasSwitchedChannels = hasJoinedNewChannel && hasParticipatedInPreviousChannel;
            var hasJoinedForTheFirstTime = previousState.VoiceChannel is null && hasJoinedNewChannel;
            var hasLeftEntirely = previousState.VoiceChannel is not null && newState.VoiceChannel is null;

            if (newState.VoiceChannel is not null)
            {
                await _mediator.Publish(new UserJoinedVoiceChannelEvent(socketUser.Id, newState.VoiceChannel.Id));
            }

            if (newState.VoiceChannel is null && previousState.VoiceChannel is not null)
            {
                await _mediator.Publish(new UserLeftVoiceChannelEvent(socketUser.Id, previousState.VoiceChannel.Id));
            }
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
