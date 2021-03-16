using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;

namespace DiscordVoiceChannelButler.Bot.Workers
{
    public class CleanUpWorker : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly BotState _botState;

        public CleanUpWorker(DiscordSocketClient client, BotState botState)
        {
            _client = client;
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

            await ScheduleRemovalAsync(previousState.VoiceChannel);
        }

        private async Task ScheduleRemovalAsync(SocketVoiceChannel voiceChannel)
        {
            await voiceChannel.DeleteAsync();
            _botState.RemoveRoom(voiceChannel.Id);
        }
    }
}