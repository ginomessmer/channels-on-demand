using Discord;
using Discord.Commands;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Models;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Commands
{
    [Group("lobby")]
    public class LobbyCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IServerService _serverService;

        public LobbyCommands(IServerService serverService)
        {
            _serverService = serverService;
        }

        [Command("register")]
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
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task Deregister(IVoiceChannel voiceChannel)
        {
            await _serverService.DeregisterLobbyAsync(voiceChannel);

            await ReplyAsync($"Voice channel `{voiceChannel.Name}` was successfully deregistered");
        }

        [Command("list")]
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
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task SetNames(IVoiceChannel voiceChannel, params string[] names)
        {
            await _serverService.ConfigureLobbySuggestedNamesAsync(voiceChannel, names);
            await ReplyAsync("Names set successfully.");
        }
    }
}