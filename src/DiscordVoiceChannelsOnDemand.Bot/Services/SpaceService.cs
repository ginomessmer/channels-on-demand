using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Models;

namespace DiscordVoiceChannelsOnDemand.Bot.Services
{
    /// <inheritdoc />
    public class SpaceService : ISpaceService
    {
        private readonly IDiscordClient _client;
        private readonly ISpaceRepository _spaceRepository;

        public SpaceService(IDiscordClient client, ISpaceRepository spaceRepository)
        {
            _client = client;
            _spaceRepository = spaceRepository;
        }

        /// <inheritdoc />
        public Task<ITextChannel> CreateSpaceAsync(IGuildUser owner) => CreateSpaceAsync(owner, Enumerable.Empty<IGuildUser>());

        /// <inheritdoc />
        public Task<ITextChannel> CreateSpaceAsync(IGuildUser owner, IEnumerable<IGuildUser> invitedUsers) =>
            CreateSpaceAsync(owner, Enumerable.Empty<IGuildUser>(), null);

        /// <inheritdoc />
        public async Task<ITextChannel> CreateSpaceAsync(IGuildUser owner, IEnumerable<IGuildUser> invitedUsers, ICategoryChannel parentCategoryChannel = null)
        {
            var guild = await _client.GetGuildAsync(owner.GuildId);
            var allowViewChannelPermission = new OverwritePermissions(viewChannel: PermValue.Allow);

            var permissions = new List<Overwrite>
            {
                // @everyone
                new(owner.Guild.EveryoneRole.Id, PermissionTarget.Role, new OverwritePermissions(viewChannel: PermValue.Deny)),
                
                // @owner
                new(owner.Id, PermissionTarget.User, allowViewChannelPermission),

                // self
                new(_client.CurrentUser.Id, PermissionTarget.User, allowViewChannelPermission)
            };
            
            // Add @invitedUsers
            permissions.AddRange(invitedUsers.Select(x =>
                new Overwrite(x.Id, PermissionTarget.User, allowViewChannelPermission)));

            // Create text channel
            var textChannel = await guild.CreateTextChannelAsync($"space-{owner.Nickname}", properties =>
            {
                properties.PermissionOverwrites = new Optional<IEnumerable<Overwrite>>(permissions);
                properties.CategoryId = parentCategoryChannel?.Id;
            });

            // Add to database
            await _spaceRepository.AddAsync(new Space
            {
                CreatorId = owner.Id.ToString(),
                TextChannelId = textChannel.Id.ToString(),
                ServerId = guild.Id.ToString()
            });

            await _spaceRepository.SaveChangesAsync();

            return textChannel;
        }

        /// <inheritdoc />
        public async Task<ITextChannel> CreateSpaceAsync(IGuildUser owner, IEnumerable<IGuildUser> invitedUsers, ulong parentCategoryChannel)
        {
            // Get category
            var guild = await _client.GetGuildAsync(owner.GuildId);

            var categories = await guild.GetCategoriesAsync();
            var categoryChannel = categories.FirstOrDefault(c => c.Id == Convert.ToUInt64(parentCategoryChannel));

            return await CreateSpaceAsync(owner, invitedUsers, categoryChannel);
        }

        /// <inheritdoc />
        public async Task<DateTime?> GetLastActivityAsync(string spaceId)
        {
            var space = await _spaceRepository.GetAsync(spaceId);
            if (space == null)
                throw new ArgumentNullException(nameof(space));

            var channel = await _client.GetChannelAsync(Convert.ToUInt64(space.TextChannelId));

            if (channel is null)
            {
                // Preemptively remove space... most likely a left over
                await _spaceRepository.RemoveAsync(space.TextChannelId);
                return null; // TODO: Throw custom exception
            }

            if (channel is not IMessageChannel messageChannel)
                throw new ArgumentException(nameof(channel), "How did you achieve this?");

            var messages = await messageChannel.GetMessagesAsync(1).FlattenAsync();
            var message = messages.FirstOrDefault();

            if (message is null)
                return channel.CreatedAt.UtcDateTime;

            var timestamp = message.EditedTimestamp ?? message.Timestamp;
            return timestamp.UtcDateTime;
        }

        /// <inheritdoc />
        public async Task<bool> ShouldRemoveSpaceAsync(string spaceId)
        {
            var lastActivity = await GetLastActivityAsync(spaceId);
            var serverThreshold = await GetSpaceTimeoutAsync(spaceId);
            return !lastActivity.HasValue || DateTime.UtcNow - lastActivity > serverThreshold;
        }

        public async Task<TimeSpan> GetSpaceTimeoutAsync(string spaceId)
        {
            var space = await _spaceRepository.GetAsync(spaceId);
            return space.Server.SpaceConfiguration.SpaceTimeoutThreshold;
        }

        /// <inheritdoc />
        public Task<IEnumerable<Space>> QueryAllSpacesAsync()
        {
            return _spaceRepository.GetAllAsync();
        }

        /// <inheritdoc />
        public async Task DecommissionAsync(string spaceId)
        {
            var space = await _spaceRepository.GetAsync(spaceId);
            if (space == null)
                throw new NullReferenceException("Space was not found");

            var id = Convert.ToUInt64(space.TextChannelId);
            var guildId = Convert.ToUInt64(space.ServerId);

            var guild = await _client.GetGuildAsync(guildId);

            var channel = await guild.GetChannelAsync(id);

            if (channel is not null)
                await channel.DeleteAsync();

            await _spaceRepository.RemoveAsync(spaceId);
            await _spaceRepository.SaveChangesAsync();
        }
    }
}