using DiscordVoiceChannelsOnDemand.Bot.Services;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Workers
{
    public class InitializeWorker : BackgroundService
    {
        private readonly IServerService _serverService;

        public InitializeWorker(IServerService serverService)
        {
            _serverService = serverService;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var guilds = await _serverService.GetAllGuildsAsync();
            foreach (var guild in guilds)
            {
                if (await _serverService.IsRegisteredAsync(guild))
                    continue;

                await _serverService.RegisterAsync(guild);
            }
        }
    }
}