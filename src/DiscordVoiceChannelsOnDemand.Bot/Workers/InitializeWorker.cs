using System;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordVoiceChannelsOnDemand.Bot.Workers
{
    public class InitializeWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public InitializeWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var serverService = scope.ServiceProvider.GetRequiredService<IServerService>();

            var guilds = await serverService.GetAllGuildsAsync();
            foreach (var guild in guilds)
            {
                if (await serverService.IsRegisteredAsync(guild))
                    continue;

                await serverService.RegisterAsync(guild);
            }
        }
    }
}