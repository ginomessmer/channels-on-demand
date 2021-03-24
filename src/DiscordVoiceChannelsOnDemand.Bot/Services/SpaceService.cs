using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace DiscordVoiceChannelsOnDemand.Bot.Services
{
    /// <inheritdoc />
    public class SpaceService : ISpaceService
    {
        private readonly IDiscordClient _client;

        public SpaceService(IDiscordClient client)
        {
            _client = client;
        }

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
            var textChannel = await guild.CreateTextChannelAsync("space", properties =>
            {
                properties.PermissionOverwrites = new Optional<IEnumerable<Overwrite>>(permissions);
                properties.CategoryId = parentCategoryChannel?.Id;
            });

            // TODO: Add to database

            return textChannel;
        }

        /// <inheritdoc />
        public async Task<ITextChannel> CreateSpaceAsync(IGuildUser owner, IEnumerable<IGuildUser> invitedUsers, ulong? parentCategoryChannel = null)
        {
            // Get category
            var guild = await _client.GetGuildAsync(owner.GuildId);

            var categories = await guild.GetCategoriesAsync();
            var categoryChannel = categories.FirstOrDefault(c => c.Id == Convert.ToUInt64(parentCategoryChannel));

            return await CreateSpaceAsync(owner, invitedUsers, categoryChannel);
        }
    }
}