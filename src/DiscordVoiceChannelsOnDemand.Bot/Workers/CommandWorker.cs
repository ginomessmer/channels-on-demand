using System.Threading;
using System.Threading.Tasks;
using DiscordVoiceChannelsOnDemand.Bot.Handlers;
using Microsoft.Extensions.Hosting;

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