using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordChannelsOnDemand.Bot.Models;

namespace DiscordChannelsOnDemand.Bot.Services
{
    /// <summary>
    /// A service class that is responsible for managing spaces.
    /// </summary>
    public interface ISpaceService
    {
        /// <summary>
        /// Creates a new private text channel - a Space.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Space> CreateSpaceAsync(IGuildUser user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="invitedUsers"></param>
        /// <returns></returns>
        Task<Space> CreateSpaceAsync(IGuildUser owner, IEnumerable<IGuildUser> invitedUsers);

        /// <summary>
        /// Creates a new private text channel - a Space.
        /// </summary>
        /// <param name="owner">The owner of the space</param>
        /// <param name="invitedUsers">All users who gain access to the space</param>
        /// <param name="parentCategoryChannel">The category that will serve as the space's parent</param>
        /// <returns></returns>
        Task<Space> CreateSpaceAsync(IGuildUser owner, IEnumerable<IGuildUser> invitedUsers,
            ICategoryChannel parentCategoryChannel = null);

        /// <inheritdoc cref="CreateSpaceAsync(Discord.IGuildUser,System.Collections.Generic.IEnumerable{Discord.IGuildUser},Discord.ICategoryChannel)"/>
        Task<Space> CreateSpaceAsync(IGuildUser owner, IEnumerable<IGuildUser> invitedUsers, ulong parentCategoryChannel);

        /// <summary>
        /// Retrieves the last activity as a date time.
        /// </summary>
        /// <param name="spaceId"></param>
        /// <returns></returns>
        Task<DateTime?> GetLastActivityAsync(string spaceId);

        /// <summary>
        /// Determines whether the space should be removed.
        /// </summary>
        /// <param name="spaceId"></param>
        /// <returns></returns>
        Task<bool> ShouldRemoveSpaceAsync(string spaceId);

        /// <summary>
        /// Queries all spaces.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Space>> QueryAllSpacesAsync();

        /// <summary>
        /// Removes a space and exports the messages if enabled.
        /// </summary>
        /// <param name="spaceId"></param>
        /// <returns></returns>
        Task DecommissionAsync(string spaceId);

        /// <summary>
        /// Applies permissions to the space.
        /// </summary>
        /// <param name="spaceId"></param>
        /// <param name="host"></param>
        /// <param name="voiceChannelUsers"></param>
        /// <returns></returns>
        Task ApplyPermissionsAsync(string spaceId, IGuildUser host, params IGuildUser[] voiceChannelUsers);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spaceId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task InviteAsync(string spaceId, IGuildUser user);
    }
}