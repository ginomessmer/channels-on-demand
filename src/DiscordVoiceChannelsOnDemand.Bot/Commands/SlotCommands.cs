using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Models;
using DiscordVoiceChannelsOnDemand.Bot.Services;

namespace DiscordVoiceChannelsOnDemand.Bot.Commands
{
    [Group("lobby")]
    public class LobbyCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IServerService _serverService;
        private readonly IServerRepository _serverRepository;

        public LobbyCommands(IServerService serverService, IServerRepository serverRepository)
        {
            _serverService = serverService;
            _serverRepository = serverRepository;
        }

        [Command("register")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task Register(IVoiceChannel voiceChannel, ICategoryChannel categoryChannel = null)
        {
            categoryChannel ??= await voiceChannel.GetCategoryAsync();

            // Get server
            var server = await _serverRepository.GetAsync(categoryChannel.GuildId.ToString());

            // Check if lobby already exists
            var id = voiceChannel.Id.ToString();
            var lobbys = await _serverRepository.QueryAllLobbysAsync();
            if (lobbys.ToList().Exists(x => x.TriggerVoiceChannelId == id))
            {
                await ReplyAsync($"Voice channel `{voiceChannel.Name}` is already configured as a lobby");
                return;
            }

            var lobby = new Lobby
            {
                TriggerVoiceChannelId = id,
                CategoryId = categoryChannel.Id.ToString()
            };

            server.Lobbys.Add(lobby);
            await _serverRepository.UpdateAsync(server);

            await ReplyAsync($"Voice channel `{voiceChannel.Name}` was successfully registered as a lobby");
        }

        [Command("unregister")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task Unregister(IVoiceChannel voiceChannel)
        {
            var lobby = await _serverRepository.FindLobbyAsync(voiceChannel.Id.ToString());
            await _serverRepository.DeleteLobbyAsync(voiceChannel.Id.ToString());

            await ReplyAsync($"Voice channel `{voiceChannel.Name}` was successfully unregistered");
        }

        [Command("list")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task List()
        {
            var guild = await _serverRepository.GetAsync(Context.Guild.Id.ToString());
            var lobbys = guild.Lobbys;

            var channels = lobbys.Select(x => Context.Guild.GetVoiceChannel(Convert.ToUInt64(x.TriggerVoiceChannelId)));
            var list = string.Join("\n", channels.Select(x => $"{x.Name}\t#{x.Id}"));
            await ReplyAsync(list);
        }

        [Command("set names")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task SetNames(IVoiceChannel voiceChannel, params string[] names)
        {
            var server = await _serverRepository.GetAsync(Context.Guild.Id.ToString());
            var lobby = server.GetLobby(voiceChannel.Id.ToString());

            if (names.Length > 1)
                lobby.RandomNames = names;
            else
            {
                lobby.RandomNames = new List<string>();
                lobby.NameFormat = names.First();
            }

            await _serverRepository.UpdateAsync(server);
        }
    }
}