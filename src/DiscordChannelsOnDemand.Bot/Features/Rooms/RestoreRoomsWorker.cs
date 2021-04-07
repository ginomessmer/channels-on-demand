using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordChannelsOnDemand.Bot.Features.Rooms
{
    /// <summary>
    /// Restores the bot in case of shutdown and purges dangling channels and cleans the database.
    /// </summary>
    public class RestoreRoomsWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RestoreRoomsWorker> _logger;

        public RestoreRoomsWorker(IServiceProvider serviceProvider,
            ILogger<RestoreRoomsWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var roomService = scope.ServiceProvider.GetRequiredService<IRoomService>();

            var voiceChannels = (await roomService.GetAllRoomVoiceChannelsAsync())
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
                    await roomService.DeleteRoomAsync(voiceChannel);
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