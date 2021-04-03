using DiscordVoiceChannelsOnDemand.Bot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
            Expression<Func<ICollection<string>, string>> convertToProviderExpression = c => string.Join(';', c);
            Expression<Func<string, ICollection<string>>> convertFromProviderExpression = c => c.Split(';', StringSplitOptions.RemoveEmptyEntries);
            
            modelBuilder.Entity<Lobby>()
                .Property(x => x.RoomNames)
                .HasConversion(
                    convertToProviderExpression,
                    convertFromProviderExpression);

            modelBuilder.Entity<Server>()
                .OwnsOne(x => x.SpaceConfiguration, builder =>
                {
                    builder.WithOwner();
                    builder.Property(x => x.SpaceDefaultNames)
                        .HasConversion(
                            convertToProviderExpression,
                            convertFromProviderExpression);
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
