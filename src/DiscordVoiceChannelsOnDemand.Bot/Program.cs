using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Options;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using DiscordVoiceChannelsOnDemand.Bot.Workers;
using LiteDB;
using Microsoft.Extensions.Configuration;

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
                    services.Configure<BotOptions>(hostContext.Configuration.GetSection("Bot"));
                    
                    // Infrastructure
                    services.AddSingleton<ILiteDatabase, LiteDatabase>(_ => new LiteDatabase("data.db"));
                    services.AddSingleton<IRoomRepository, LiteDbRoomRepository>();

                    // Bot Services
                    services.AddSingleton<IRoomService, SocketRoomService>();
                    services.AddSingleton<IVoiceChannelService, SocketVoiceChannelService>();

                    services.AddSingleton<IDiscordClient, DiscordSocketClient>(sp => sp.GetRequiredService<DiscordSocketClient>())
                        .AddSingleton<DiscordSocketClient>(sp => CreateDiscordSocketClient(hostContext));

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
