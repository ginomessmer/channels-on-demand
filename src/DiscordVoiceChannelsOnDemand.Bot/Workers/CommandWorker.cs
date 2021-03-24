using System.Text.Json.Serialization;
using DiscordVoiceChannelsOnDemand.Bot.Commands;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Workers
{
    public class CommandWorker : BackgroundService
    {
        private readonly CommandHandler _commandHandler;

        public CommandWorker(CommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        /// <inheritdoc />
        protected override Task ExecuteAsync(CancellationToken stoppingToken) => _commandHandler.InstallCommandsAsync();
    }
}