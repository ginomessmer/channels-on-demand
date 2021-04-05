using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Humanizer;

namespace DiscordVoiceChannelsOnDemand.Bot.Commands
{
    [Group("space")]
    [RequireContext(ContextType.Guild)]
    public class SpaceCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IServerService _serverService;
        private readonly ISpaceService _spaceService;

        public SpaceCommands(IServerService serverService, ISpaceService spaceService)
        {
            _serverService = serverService;
            _spaceService = spaceService;
        }

        [Command("enable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetupSpace(ICategoryChannel categoryChannel)
        {
            await _serverService.EnableSpacesAsync(categoryChannel);
            await Context.Message.AddReactionAsync(new Emoji("✅"));
        }

        [Command("disable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DisableSpace()
        {
            await _serverService.DisableSpacesAsync(Context.Guild);
            await Context.Message.AddReactionAsync(new Emoji("✅"));
        }

        [Command("create")]
        [Alias("new")]
        [Summary("Creates a new private text channel for you and all users you mention")]
        public async Task CreateSpace(params IGuildUser[] users)
        {
            // Check whether space is set up/enabled on the server
            var server = await _serverService.GetAsync(Context.Guild);
            if (!server.SpaceConfiguration.IsSpacesEnabled)
            {
                // TODO: Reply
                return;
            }

            // Get category
            ulong? categoryId = null;

            if (!string.IsNullOrEmpty(server.SpaceConfiguration.SpaceCategoryId))
                categoryId = Convert.ToUInt64(server.SpaceConfiguration.SpaceCategoryId);

            // Create new text channel
            var channel = categoryId is null ? await _spaceService.CreateSpaceAsync(Context.User as IGuildUser, users)
                : await _spaceService.CreateSpaceAsync(Context.User as IGuildUser, users, (ulong) categoryId);

            // Reply
            var embed = new EmbedBuilder()
                .WithTitle("Your space is ready to go")
                .WithDescription(new StringBuilder()
                    .Append($"Your space is only visible to you and everyone you invited " +
                                $"(that is {string.Join(", ", users.Select(x => x.Nickname))}). ")
                    .Append($"It will expire after {server.SpaceConfiguration.SpaceTimeoutThreshold.Humanize()} of inactivity.")
                    .ToString())
                .WithThumbnailUrl(Context.User.GetAvatarUrl());

            await channel.SendMessageAsync($"{Context.User.Mention} :wave:", embed: embed.Build());
            await Context.Message.AddReactionAsync(new Emoji("✅"));
        }
    }
}