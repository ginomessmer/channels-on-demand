using Discord;

namespace DiscordVoiceChannelsOnDemand.Bot.Data
{
    public record LobbyResult(IVoiceChannel VoiceChannel, ICategoryChannel CategoryChannel);
}