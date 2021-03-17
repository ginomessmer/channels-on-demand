using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordVoiceChannelButler.Bot.Infrastructure;
using DiscordVoiceChannelButler.Bot.Services;
using Microsoft.Extensions.Hosting;

namespace DiscordVoiceChannelButler.Bot.Workers
{
    /// <summary>
    /// This worker removes empty voice channels created by the butler.
    /// </summary>
    public class CleanUpWorker : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly IRoomService _roomService;
        private readonly IRoomRepository _roomRepository;

        public CleanUpWorker(DiscordSocketClient client, IRoomService roomService, IRoomRepository roomRepository)
        {
            _client = client;
            _roomService = roomService;
            _roomRepository = roomRepository;
        }

        /// <inheritdoc />
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _client.UserVoiceStateUpdated += ClientOnUserVoiceStateUpdated;
            return Task.CompletedTask;
        }

        private async Task ClientOnUserVoiceStateUpdated(SocketUser arg1, SocketVoiceState previousState, SocketVoiceState newState)
        {
            if (previousState.VoiceChannel is null)
                return;

            // Check if its part of Rooms
            if (!await _roomRepository.ExistsAsync(previousState.VoiceChannel.Id.ToString()))
                return;

            // Handle disconnect
            if (previousState.VoiceChannel.Users.Count > 0)
                return;

            await _roomService.DeleteRoomAsync(previousState.VoiceChannel);
        }
    }
}