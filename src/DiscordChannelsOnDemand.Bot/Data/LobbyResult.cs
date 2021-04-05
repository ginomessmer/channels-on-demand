using Discord;

namespace DiscordChannelsOnDemand.Bot.Data
{
    public record LobbyResult(IVoiceChannel VoiceChannel, ICategoryChannel CategoryChannel);
}