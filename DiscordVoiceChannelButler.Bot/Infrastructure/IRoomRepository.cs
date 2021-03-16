﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordVoiceChannelButler.Bot.Models;

namespace DiscordVoiceChannelButler.Bot.Infrastructure
{
    public interface IRoomRepository
    {
        /// <summary>
        /// Adds a new room to the repository.
        /// </summary>
        /// <param name="voiceChannelId">The voice channel's ID</param>
        /// <param name="hostUserId">The host's user ID</param>
        /// <returns>The persisted room model instance</returns>
        Task<Room> AddAsync(ulong voiceChannelId, ulong hostUserId);

        /// <summary>
        /// Removes a room entirely.
        /// </summary>
        /// <param name="voiceChannelId"></param>
        /// <returns></returns>
        Task RemoveAsync(ulong voiceChannelId);

        /// <summary>
        /// Checks whether a voice channel exists in the repository. Returns true if that's the case.
        /// </summary>
        /// <param name="voiceChannelId"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(ulong voiceChannelId);
    }
}
