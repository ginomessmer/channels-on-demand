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
        private readonly IServerService _serverService;
        private readonly IServerRepository _serverRepository;

        public InitializeWorker(IServerService serverService, IServerRepository serverRepository)
        {
            _serverService = serverService;
            _serverRepository = serverRepository;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var guilds = await _serverService.GetAllGuildsAsync();
            foreach (var guild in guilds)
            {
                if (await _serverRepository.ExistsAsync(guild.Id.ToString()))
                    continue;

                await _serverRepository.AddAsync(new Server
                {
                    GuildId = guild.Id.ToString(),
                    Lobbies = new List<Lobby>()
                });
            }
        }
    }
}