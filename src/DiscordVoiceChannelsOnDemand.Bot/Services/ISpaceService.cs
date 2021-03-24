using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordVoiceChannelsOnDemand.Bot.Services
{
    /// <summary>
    /// A service class that is responsible for managing spaces.
    /// </summary>
    public interface ISpaceService
    {
        /// <summary>
        /// Creates a new private text channel - a Space.
        /// </summary>
        /// <param name="owner">The owner of the space</param>
        /// <param name="invitedUsers">All users who gain access to the space</param>
        /// <param name="parentCategoryChannel">The category that will serve as the space's parent</param>
        /// <returns></returns>
        Task<ITextChannel> CreateSpaceAsync(IGuildUser owner, IEnumerable<IGuildUser> invitedUsers,
            ICategoryChannel parentCategoryChannel = null);

        /// <inheritdoc cref="CreateSpaceAsync(Discord.IGuildUser,System.Collections.Generic.IEnumerable{Discord.IGuildUser},Discord.ICategoryChannel)"/>
        Task<ITextChannel> CreateSpaceAsync(IGuildUser owner, IEnumerable<IGuildUser> invitedUsers,
            ulong? parentCategoryChannel = null);
    }
}