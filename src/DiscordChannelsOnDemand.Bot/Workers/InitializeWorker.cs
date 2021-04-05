using DiscordChannelsOnDemand.Bot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordChannelsOnDemand.Bot.Workers
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