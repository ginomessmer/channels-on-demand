namespace DiscordVoiceChannelsOnDemand.Bot.Abstractions
{
    public interface ILobby
    {
        /// <summary>
        /// The voice channel's ID that will create a new room on demand.
        /// </summary>
        string TriggerVoiceChannelId { get; set; }

        /// <summary>
        /// The category ID that will be used to append new rooms to.
        /// </summary>
        string CategoryId { get; set; }

        /// <summary>
        /// Suggests a room name that is the child of this lobby.
        /// </summary>
        /// <returns></returns>
        string SuggestRoomName();

        /// <summary>
        /// Indicated whether the lobby has a parent category where all new rooms will be appended to.
        /// </summary>
        bool HasCategory { get; }
    }
}