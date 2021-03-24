using DiscordVoiceChannelsOnDemand.Bot.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure.EntityFramework
{
    public sealed class BotDbContext : DbContext
    {
        public DbSet<Server> Servers { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Space> Spaces { get; set; }

        /// <inheritdoc />
        private BotDbContext()
        {
        }

        /// <inheritdoc />
        public BotDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
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
