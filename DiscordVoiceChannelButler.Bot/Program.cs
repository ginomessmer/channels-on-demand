using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelButler.Bot.Infrastructure;
using DiscordVoiceChannelButler.Bot.Options;
using DiscordVoiceChannelButler.Bot.Services;
using DiscordVoiceChannelButler.Bot.Workers;
using LiteDB;
using Microsoft.Extensions.Configuration;

namespace DiscordVoiceChannelButler.Bot
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

                    services.AddSingleton<IDiscordClient, DiscordSocketClient>(sp => sp.GetRequiredService<DiscordSocketClient>())
                        .AddSingleton<DiscordSocketClient>(sp =>
                        {
                            var token = hostContext.Configuration.GetConnectionString("DiscordBotToken");
                            var client = new DiscordSocketClient();

                            Task.WaitAll(client.LoginAsync(TokenType.Bot, token), client.StartAsync());

                            return client;
                        });

                    services.AddHostedService<OnDemandRoomWorker>();
                    services.AddHostedService<CleanUpWorker>();
                });
    }
}
