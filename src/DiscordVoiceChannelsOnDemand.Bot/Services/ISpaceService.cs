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
        /// <param name="owner"></param>
        /// <param name="invitedUsers"></param>
        /// <param name="parentCategoryChannel"></param>
        /// <returns></returns>
        Task<ITextChannel> CreateSpaceAsync(IGuildUser owner, IEnumerable<IGuildUser> invitedUsers,
            ICategoryChannel parentCategoryChannel = null);
    }
}