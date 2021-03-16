using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
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
                    services.AddHostedService<Worker>();

                    services.Configure<BotOptions>(hostContext.Configuration.GetSection("Bot"));

                    services.AddSingleton<IDiscordClient, DiscordSocketClient>(sp => sp.GetRequiredService<DiscordSocketClient>())
                        .AddSingleton<DiscordSocketClient>(sp =>
                        {
                            var client = new DiscordSocketClient();
                            Task.WaitAll(
                                client.LoginAsync(TokenType.Bot, hostContext.Configuration.GetConnectionString("DiscordBotToken")),
                                client.StartAsync());

                            return client;
                        });
                });
    }
}
