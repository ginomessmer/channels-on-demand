namespace DiscordVoiceChannelsOnDemand.Bot.Options
{
    public class BotOptions
    {
        /// <summary>
        /// The category where new rooms should be created.
        /// </summary>
        public ulong CategoryId { get; set; }

        /// <summary>
        /// The entry voice channel that triggers new rooms.
        /// </summary>
        public ulong GatewayVoiceChannelId { get; set; }

        /// <summary>
        /// The room's naming scheme.
        /// </summary>
        public string RoomNameFormat { get; set; } = "Meeting Room {0}";
    }
}