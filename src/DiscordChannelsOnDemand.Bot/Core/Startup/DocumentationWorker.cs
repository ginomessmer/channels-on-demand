using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace DiscordChannelsOnDemand.Bot.Core.Startup
{
    /// <summary>
    /// It's like a swagger doc generator but better.
    /// </summary>
    public class DocumentationWorker : BackgroundService
    {
        private readonly CommandService _commandService;

        public DocumentationWorker(CommandService commandService)
        {
            _commandService = commandService;
        }

        /// <inheritdoc />
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var commands = _commandService.Commands
                .OrderBy(a => a.Module.Name)
                .Select(a => new
                {
                    a.Name,
                    a.Summary,
                    Module = a.Module.Name,
                    Parameters = a.Parameters.Select(x => new
                    {
                        x.Name,
                        x.Summary,
                        x.DefaultValue,
                        x.IsOptional,
                        Type = x.Type.Name
                    })
                });

            // JSON
            var json = JsonConvert.SerializeObject(commands, Formatting.Indented);
            return File.WriteAllTextAsync("commands.json", json, stoppingToken);
        }
    }
}