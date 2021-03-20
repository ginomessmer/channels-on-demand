using System;
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

        /// <inheritdoc />
        protected BotDbContext()
        {
        }

        /// <inheritdoc />
        public BotDbContext(DbContextOptions options) : base(options)
        {
        }
        
        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lobby>()
                .Property(x => x.RoomNames)
                .HasConversion(
                    c => string.Join(';', c),
                    c => c.Split(';', StringSplitOptions.RemoveEmptyEntries));

            base.OnModelCreating(modelBuilder);
        }
    }
}
