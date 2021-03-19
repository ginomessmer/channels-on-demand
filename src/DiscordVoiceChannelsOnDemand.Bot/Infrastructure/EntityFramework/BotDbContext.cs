using System.Linq;
using System.Text;
using DiscordVoiceChannelsOnDemand.Bot.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure.EntityFramework
{
    public class BotDbContext : DbContext
    {
        public DbSet<Server> Servers { get; set; }

        public DbSet<Room> Rooms { get; set; }
    }
}
