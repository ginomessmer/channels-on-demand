using Discord.WebSocket;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Workers
{
    /// <summary>
    /// Restores the bot in case of shutdown and purges dangling channels and cleans the database.
    /// </summary>
    public class RestoreWorker : BackgroundService
    {
        private readonly IRoomService _roomService;
        private readonly ILogger<RestoreWorker> _logger;

        public RestoreWorker(IRoomService roomService,
            ILogger<RestoreWorker> logger)
        {
            _roomService = roomService;
            _logger = logger;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var voiceChannels = (await _roomService.GetAllRoomVoiceChannelsAsync())
                .Cast<SocketVoiceChannel>();

            foreach (var voiceChannel in voiceChannels)
            {
                if (voiceChannel is null)
                    continue;

                var userCount = voiceChannel.Users.Count;
                if (userCount > 0)
                    continue;

                try
                {
                    await _roomService.DeleteRoomAsync(voiceChannel);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Couldn't purge voice channel {VoiceChannel}. Skipping...", voiceChannel);
                    continue;
                }

                _logger.LogInformation("Purged voice channel {VoiceChannel} from last bot session", voiceChannel);
            }
        }
    }
}