using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Workers
{
    /// <summary>
    /// This worker creates a new room on demand whenever someone joins
    /// the lobby voice channel.
    /// </summary>
    public class CreateRoomWorker : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly IRoomService _roomService;
        private readonly IServerService _serverService;
        private readonly ILogger<CreateRoomWorker> _logger;

        public CreateRoomWorker(DiscordSocketClient client,
            IRoomService roomService,
            IServerService serverService,
            ILogger<CreateRoomWorker> logger)
        {
            _client = client;
            _roomService = roomService;
            _serverService = serverService;
            _logger = logger;
        }

        private Task ClientOnUserVoiceStateUpdated(SocketUser socketUser,
            SocketVoiceState previousState, SocketVoiceState newState)
        {
            var voiceChannel = newState.VoiceChannel ?? previousState.VoiceChannel;
            return HandleAsync(socketUser, voiceChannel);
        }

        private async Task HandleAsync(IUser socketUser, IVoiceChannel voiceChannel)
        {
            // Check if user is typeof(SocketGuildUser)
            if (socketUser is not SocketGuildUser user)
                return;

            if (voiceChannel is null)
                return;

            // Check whether lobby exists for the channel
            if (!await _serverService.IsLobbyRegisteredAsync(voiceChannel))
                return;

            // Create new voice channel
            var lobby = await _serverService.GetLobbyAsync(voiceChannel);
            var channel = await _roomService.CreateNewRoomAsync(user, lobby);
            _logger.LogInformation("Created new room {VoiceChannel} for user {User}", channel, user);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _client.UserVoiceStateUpdated += ClientOnUserVoiceStateUpdated;
            return Task.CompletedTask;
        }
    }
}
