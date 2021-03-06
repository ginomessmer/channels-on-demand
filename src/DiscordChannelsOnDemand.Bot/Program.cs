using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordChannelsOnDemand.Bot.Commands;
using DiscordChannelsOnDemand.Bot.Infrastructure;
using DiscordChannelsOnDemand.Bot.Infrastructure.EntityFramework;
using DiscordChannelsOnDemand.Bot.Services;
using DiscordChannelsOnDemand.Bot.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace DiscordChannelsOnDemand.Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigureLogging();
            CreateHostBuilder(args).Build().Run();
        }

        private static void ConfigureLogging() =>
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File("logs/bot.log")
                .WriteTo.Console()
                .CreateLogger();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(x => x.AddUserSecrets<Program>())
                .ConfigureServices((hostContext, services) =>
                {
                    // Infrastructure
                    services.AddDbContext<BotDbContext>(builder => 
                        builder.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultDbContext")));
                    services.AddScoped<IServerRepository, EfCoreServerRepository>();
                    services.AddScoped<IRoomRepository, EfCoreRoomRepository>();
                    services.AddScoped<ISpaceRepository, EfCoreSpaceRepository>();

                    // Bot Services
                    services.AddScoped<IServerService, ServerService>();
                    services.AddScoped<IVoiceChannelService, VoiceChannelService>();
                    services.AddScoped<IRoomService, RoomService>();
                    services.AddScoped<ISpaceService, SpaceService>();

                    // Discord
                    services.AddSingleton<DiscordSocketClient>(sp => CreateDiscordSocketClient(hostContext));
                    services.AddSingleton<IDiscordClient, DiscordSocketClient>(sp => sp.GetRequiredService<DiscordSocketClient>());
                    services.AddSingleton<CommandServiceConfig>();
                    services.AddSingleton<CommandService>();
                    services.AddSingleton<CommandHandler>();

                    // Workers
                    services.AddHostedService<CommandWorker>();
                    services.AddHostedService<InitializeWorker>();
                    services.AddHostedService<ServerRegistrationWorker>();
                    services.AddHostedService<RestoreWorker>();
                    services.AddHostedService<CreateRoomWorker>();
                    services.AddHostedService<RoomPurgeWorker>();
                    services.AddHostedService<SpacePurgeWorker>();
                    services.AddHostedService<SpaceLobbySyncWorker>();
                    services.AddHostedService<DocumentationWorker>();

                    // Misc
                    services.AddAutoMapper(typeof(Program));
                })
                .UseSerilog();

        private static DiscordSocketClient CreateDiscordSocketClient(HostBuilderContext hostContext)
        {
            var readyEvent = new AutoResetEvent(false);

            var token = hostContext.Configuration.GetConnectionString("DiscordBotToken");

            var client = new DiscordSocketClient();
            client.Ready += () => Task.FromResult(readyEvent.Set());

            Task.WaitAll(client.LoginAsync(TokenType.Bot, token), client.StartAsync());

            readyEvent.WaitOne(TimeSpan.FromSeconds(30));
            return client;
        }
    }
}
