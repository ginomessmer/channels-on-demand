using System.Threading;
using System.Threading.Tasks;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Microsoft.Extensions.Hosting;

namespace DiscordVoiceChannelsOnDemand.Bot.Workers
{
    /// <summary>
    /// Restores the bot in case of shutdown and purges dangling channels and cleans the database.
    /// </summary>
    public class RestoreWorker : BackgroundService
    {
        private readonly IRoomService _roomService;
        private readonly IVoiceChannelService _voiceChannelService;

        public RestoreWorker(IRoomService roomService, IVoiceChannelService voiceChannelService)
        {
            _roomService = roomService;
            _voiceChannelService = voiceChannelService;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Purge dangling channels
            var allChannels = await _voiceChannelService.GetAllAsync();
        }
    }
}