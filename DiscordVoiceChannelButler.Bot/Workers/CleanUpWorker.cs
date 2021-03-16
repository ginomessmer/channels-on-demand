using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordVoiceChannelButler.Bot.Services;
using Microsoft.Extensions.Hosting;

namespace DiscordVoiceChannelButler.Bot.Workers
{
    public class CleanUpWorker : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly IRoomService _roomService;
        private readonly BotState _botState;

        public CleanUpWorker(DiscordSocketClient client, IRoomService roomService, BotState botState)
        {
            _client = client;
            _roomService = roomService;
            _botState = botState;
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
            if (!_botState.ExistsRoom(previousState.VoiceChannel.Id))
                return;

            // Handle disconnect
            if (previousState.VoiceChannel.Users.Count > 0)
                return;

            await _roomService.DeleteRoomAsync(previousState.VoiceChannel);
        }
    }
}