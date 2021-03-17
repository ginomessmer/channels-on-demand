using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelsOnDemand.Bot.Options;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Workers
{
    /// <summary>
    /// This worker creates a new room on demand whenever someone joins
    /// the <seealso cref="BotOptions.GatewayVoiceChannelId"/> voice channel.
    /// </summary>
    public class OnDemandRoomWorker : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly IRoomService _roomService;
        private readonly BotOptions _options;
        private readonly ILogger<OnDemandRoomWorker> _logger;

        public OnDemandRoomWorker(DiscordSocketClient client, IRoomService roomService,
            IOptions<BotOptions> options, ILogger<OnDemandRoomWorker> logger)
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
