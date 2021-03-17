using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordVoiceChannelsOnDemand.Bot.Workers
{
    /// <summary>
    /// Restores the bot in case of shutdown and purges dangling channels and cleans the database.
    /// </summary>
    public class RestoreWorker : BackgroundService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomService _roomService;
        private readonly IVoiceChannelService _voiceChannelService;
        private readonly ILogger<RestoreWorker> _logger;

        public RestoreWorker(IRoomRepository roomRepository,
            IRoomService roomService,
            IVoiceChannelService voiceChannelService,
            ILogger<RestoreWorker> logger)
        {
            _roomRepository = roomRepository;
            _roomService = roomService;
            _voiceChannelService = voiceChannelService;
            _logger = logger;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Purge dangling channels
            var allVoiceChannels = (await _voiceChannelService.GetAllAsync()).ToList();
            var allRooms = (await _roomRepository.GetAllAsync()).ToList();

            var selectVoiceChannels = allVoiceChannels
                .Where(x => allRooms
                    .Exists(z => z.ChannelId == x.Id.ToString()))
                .Cast<SocketVoiceChannel>();

            foreach (var voiceChannel in selectVoiceChannels)
            {
                var userCount = voiceChannel.Users.Count;
                if (userCount > 0)
                    continue;

                await _roomService.DeleteRoomAsync(voiceChannel.Id, voiceChannel.Guild.Id);
                _logger.LogInformation("Purged voice channel {VoiceChannel}", voiceChannel);
            }
        }
    }
}