using Discord.WebSocket;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Workers
{
    /// <summary>
    /// This worker removes empty voice channels created by the butler.
    /// </summary>
    public class PurgeRoomWorker : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly IRoomService _roomService;
        private readonly ILogger<PurgeRoomWorker> _logger;

        public PurgeRoomWorker(DiscordSocketClient client,
            IRoomService roomService,
            ILogger<PurgeRoomWorker> logger)
        {
            _client = client;
            _roomService = roomService;
            _logger = logger;
        }

        /// <inheritdoc />
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _client.UserVoiceStateUpdated += ClientOnUserVoiceStateUpdated;
            return Task.CompletedTask;
        }

        private async Task ClientOnUserVoiceStateUpdated(SocketUser user, SocketVoiceState previousState, SocketVoiceState newState)
        {
            var voiceChannel = previousState.VoiceChannel;

            if (voiceChannel is null)
                return;

            // Check if its a room
            if (!await _roomService.ExistsAsync(voiceChannel))
                return;

            // Handle disconnect
            if (voiceChannel.Users.Count > 0)
                return;

            await _roomService.DeleteRoomAsync(voiceChannel);
            _logger.LogInformation($"Purged room {voiceChannel}");
        }
    }
}