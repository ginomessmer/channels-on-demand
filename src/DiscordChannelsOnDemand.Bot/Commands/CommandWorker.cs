using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace DiscordChannelsOnDemand.Bot.Commands
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