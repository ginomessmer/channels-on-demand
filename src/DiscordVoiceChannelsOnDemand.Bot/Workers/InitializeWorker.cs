using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Models;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Microsoft.Extensions.Hosting;

namespace DiscordVoiceChannelsOnDemand.Bot.Workers
{
    public class InitializeWorker : BackgroundService
    {
        private readonly ITenantService _tenantService;
        private readonly ITenantRepository _tenantRepository;

        public InitializeWorker(ITenantService tenantService, ITenantRepository tenantRepository)
        {
            _tenantService = tenantService;
            _tenantRepository = tenantRepository;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var guilds = await _tenantService.GetAllGuildsAsync();
            foreach (var guild in guilds)
            {
                if (await _tenantRepository.ExistsAsync(guild.Id.ToString()))
                    continue;

                await _tenantRepository.AddAsync(new Tenant
                {
                    GuildId = guild.Id.ToString(),
                    Slots = new List<Slot>()
                });
            }
        }
    }
}