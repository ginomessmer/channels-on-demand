using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelButler.Bot.Options;
using DiscordVoiceChannelButler.Bot.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscordVoiceChannelButler.Bot.Workers
{
    public class RoomWorker : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly IRoomService _roomService;
        private readonly BotOptions _options;
        private readonly ILogger<RoomWorker> _logger;

        public RoomWorker(DiscordSocketClient client, IRoomService roomService,
            IOptions<BotOptions> options, ILogger<RoomWorker> logger)
        {
            _client = client;
            _roomService = roomService;
            _options = options.Value;
            _logger = logger;
        }

        private Task ClientOnUserVoiceStateUpdated(SocketUser socketUser,
            SocketVoiceState previousState, SocketVoiceState newState) => HandleAsync(socketUser);

        private async Task HandleAsync(IUser socketUser)
        {
            // Check if user is typeof(SocketGuildUser)
            if (socketUser is not SocketGuildUser user)
                return;

            // Check voice channel
            if (user.VoiceChannel?.Id != _options.GatewayVoiceChannelId)
                return;

            // Create new voice channel
            var channel = await _roomService.CreateNewRoomAsync(user);
            _logger.LogInformation("Created new room {VoiceChannel} for user {User}", channel, user);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _client.UserVoiceStateUpdated += ClientOnUserVoiceStateUpdated;
            return Task.CompletedTask;
        }
    }
}
