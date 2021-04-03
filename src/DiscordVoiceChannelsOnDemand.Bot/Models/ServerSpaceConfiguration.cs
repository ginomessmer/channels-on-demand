using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DiscordVoiceChannelsOnDemand.Bot.Models
{
    public class ServerSpaceConfiguration
    {
        /// <summary>
        /// The category's ID for new spaces.
        /// </summary>
        public string SpaceCategoryId { get; set; }

        /// <summary>
        /// The duration for spaces to expire.
        /// </summary>
        public TimeSpan SpaceTimeoutThreshold { get; set; } = TimeSpan.FromDays(1);

        /// <summary>
        /// Default names for spaces.
        /// </summary>
        [Obsolete("Not implemented yet")]
        public ICollection<string> SpaceDefaultNames { get; set; } = new List<string> { "space-{0}" };

        /// <summary>
        /// Determines whether Spaces is enabled on the server.
        /// </summary>
        public bool IsSpacesEnabled => !string.IsNullOrEmpty(SpaceCategoryId);
    }
}