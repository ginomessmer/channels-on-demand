namespace DiscordVoiceChannelButler.Bot.Options
{
    public class BotOptions
    {
        public ulong CategoryId { get; set; }

        public ulong GatewayVoiceChannelId { get; set; }

        public string RoomName { get; set; } = "Meeting Room";
    }
}