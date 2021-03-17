using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using DiscordVoiceChannelsOnDemand.Bot.Workers;
using LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordVoiceChannelsOnDemand.Bot.Handlers;

namespace DiscordVoiceChannelsOnDemand.Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(x => x.AddUserSecrets<Program>())
                .ConfigureServices((hostContext, services) =>
                {
                    // Infrastructure
                    services.AddSingleton<ILiteDatabase, LiteDatabase>(_ => new LiteDatabase("data.db"));
                    services.AddSingleton<ITenantRepository, LiteDbTenantRepository>();
                    services.AddSingleton<IRoomRepository, LiteDbRoomRepository>();

                    // Bot Services
                    services.AddSingleton<ITenantService, TenantService>();
                    services.AddSingleton<IRoomService, RoomService>();
                    services.AddSingleton<IVoiceChannelService, VoiceChannelService>();

                    // Discord
                    services.AddSingleton<DiscordSocketClient>(sp => CreateDiscordSocketClient(hostContext));
                    services.AddSingleton<IDiscordClient, DiscordSocketClient>(sp => sp.GetRequiredService<DiscordSocketClient>());
                    services.AddSingleton<CommandServiceConfig>();
                    services.AddSingleton<CommandService>();
                    services.AddSingleton<CommandHandler>();

                    // Workers
                    services.AddHostedService<CommandWorker>();
                    services.AddHostedService<InitializeWorker>();
                    services.AddHostedService<RestoreWorker>();
                    services.AddHostedService<OnDemandRoomWorker>();
                    services.AddHostedService<CleanUpWorker>();
                });

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
