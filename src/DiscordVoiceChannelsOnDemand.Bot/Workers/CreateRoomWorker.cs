using System;
using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordVoiceChannelsOnDemand.Bot.Workers
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
            var unitOfWork = scope.ServiceProvider.GetRequiredService<OnDemandUnitOfWork>();

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
            _logger.LogInformation("Created new room {VoiceChannel} for user {User}", channel, user);
        }
    }
}
