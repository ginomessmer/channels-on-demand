using Discord;

namespace DiscordChannelsOnDemand.Bot.Core.Data
{
    public record LobbyResult(IVoiceChannel VoiceChannel, ICategoryChannel CategoryChannel);
}