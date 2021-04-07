using DiscordChannelsOnDemand.Bot.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordChannelsOnDemand.Bot.Workers
{
    public class SpacePurgeWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SpacePurgeWorker> _logger;

        public SpacePurgeWorker(IServiceProvider serviceProvider, ILogger<SpacePurgeWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        #region Overrides of BackgroundService

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                _logger.LogInformation("Starting sweep...");

                using var scope = _serviceProvider.CreateScope();
                var spaceService = scope.ServiceProvider.GetRequiredService<ISpaceService>();

                var spaces = await spaceService.QueryAllSpacesAsync();

                foreach (var space in spaces)
                {
                    var remove = await spaceService.ShouldRemoveSpaceAsync(space.TextChannelId);
                    if (!remove)
                        continue;

                    await spaceService.DecommissionAsync(space.TextChannelId);
                    _logger.LogInformation("Purged space {Space}", space.TextChannelId);
                }

                _logger.LogInformation("Sweep finished");
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        #endregion
    }
}