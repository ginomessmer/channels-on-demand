using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordChannelsOnDemand.Bot.Core.Services;

namespace DiscordChannelsOnDemand.Bot.Commands.Modules.Lobby
{
    [Group("lobby")]
    [Summary("Helps you creating new lobbies")]
    public class LobbyCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IServerService _serverService;

        public LobbyCommands(IServerService serverService)
        {
            _serverService = serverService;
        }

        [Command("register")]
        [Summary("Registers the voice channel as a new lobby")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task Register(IVoiceChannel voiceChannel, ICategoryChannel categoryChannel = null)
        {
            categoryChannel ??= await voiceChannel.GetCategoryAsync();
            
            // Check if lobby already exists
            if (await _serverService.IsLobbyRegisteredAsync(voiceChannel))
            {
                await ReplyAsync($"Voice channel `{voiceChannel.Name}` is already configured as a lobby");
                return;
            }

            await _serverService.RegisterLobbyAsync(voiceChannel, categoryChannel);

            await ReplyAsync($"Voice channel `{voiceChannel.Name}` was successfully registered as a lobby");
        }

        [Command("deregister")]
        [Summary("Deregisters the channel")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task Deregister(IVoiceChannel voiceChannel)
        {
            await _serverService.DeregisterLobbyAsync(voiceChannel);

            await ReplyAsync($"Voice channel `{voiceChannel.Name}` was successfully deregistered");
        }

        [Command("list")]
        [Summary("Lists all lobbies on the server")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task List()
        {
            var lobbies = await _serverService.ListLobbiesAsync(Context.Guild);
            var lobbyResults = lobbies.ToList();

            var message = lobbyResults.Any()
                ? string.Join("\n", lobbyResults.Select(x => $"{x.VoiceChannel.Name} (#{x.VoiceChannel.Id})"))
                : "No lobbies";

            await ReplyAsync(message);
        }

        [Command("set names")]
        [Summary("Lets you set possible names for rooms that will be chosen at random")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task SetNames(IVoiceChannel voiceChannel, params string[] names)
        {
            await _serverService.ConfigureLobbySuggestedNamesAsync(voiceChannel, names);
            await ReplyAsync("Names set successfully.");
        }

        [Command("set space autocreate")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task SetSpaceAutoCreate(IVoiceChannel voiceChannel, bool autoCreate = true)
        {
            var lobby = await _serverService.GetLobbyAsync(voiceChannel);
            await _serverService.ConfigureSpaceAsync(lobby, x =>
            {
                x.AutoCreate = autoCreate;
            });
            await ReplyAsync("Auto create set"); // TODO: Better response message
        }
    }
}