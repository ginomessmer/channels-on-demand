using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;

namespace DiscordChannelsOnDemand.Tests.SeedWork
{
    public class GuildFake : IGuild
    {
        #region Implementation of IDeletable

        /// <inheritdoc />
        public Task DeleteAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of IEntity<ulong>

        /// <inheritdoc />
        public ulong Id { get; set; }

        #endregion

        #region Implementation of ISnowflakeEntity

        /// <inheritdoc />
        public DateTimeOffset CreatedAt { get; set; }

        #endregion

        #region Implementation of IGuild

        /// <inheritdoc />
        public Task ModifyAsync(Action<GuildProperties> func, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task ModifyEmbedAsync(Action<GuildEmbedProperties> func, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task ModifyWidgetAsync(Action<GuildWidgetProperties> func, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task ReorderChannelsAsync(IEnumerable<ReorderChannelProperties> args, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task ReorderRolesAsync(IEnumerable<ReorderRoleProperties> args, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task LeaveAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IReadOnlyCollection<IBan>> GetBansAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IBan> GetBanAsync(IUser user, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IBan> GetBanAsync(ulong userId, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task AddBanAsync(IUser user, int pruneDays = 0, string reason = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task AddBanAsync(ulong userId, int pruneDays = 0, string reason = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task RemoveBanAsync(IUser user, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task RemoveBanAsync(ulong userId, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IReadOnlyCollection<IGuildChannel>> GetChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IGuildChannel> GetChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IReadOnlyCollection<ITextChannel>> GetTextChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<ITextChannel> GetTextChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IReadOnlyCollection<IVoiceChannel>> GetVoiceChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IReadOnlyCollection<ICategoryChannel>> GetCategoriesAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IVoiceChannel> GetVoiceChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IVoiceChannel> GetAFKChannelAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<ITextChannel> GetSystemChannelAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<ITextChannel> GetDefaultChannelAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IGuildChannel> GetEmbedChannelAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IGuildChannel> GetWidgetChannelAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<ITextChannel> GetRulesChannelAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<ITextChannel> GetPublicUpdatesChannelAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<ITextChannel> CreateTextChannelAsync(string name, Action<TextChannelProperties> func = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IVoiceChannel> CreateVoiceChannelAsync(string name, Action<VoiceChannelProperties> func = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<ICategoryChannel> CreateCategoryAsync(string name, Action<GuildChannelProperties> func = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IReadOnlyCollection<IVoiceRegion>> GetVoiceRegionsAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IReadOnlyCollection<IGuildIntegration>> GetIntegrationsAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IGuildIntegration> CreateIntegrationAsync(ulong id, string type, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IReadOnlyCollection<IInviteMetadata>> GetInvitesAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IInviteMetadata> GetVanityInviteAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IRole GetRole(ulong id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IRole> CreateRoleAsync(string name, GuildPermissions? permissions = null, Color? color = null, bool isHoisted = false,
            RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IRole> CreateRoleAsync(string name, GuildPermissions? permissions = null, Color? color = null, bool isHoisted = false,
            bool isMentionable = false, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IGuildUser> AddGuildUserAsync(ulong userId, string accessToken, Action<AddGuildUserProperties> func = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IReadOnlyCollection<IGuildUser>> GetUsersAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            IReadOnlyCollection<IGuildUser> collection = new ReadOnlyCollection<IGuildUser>(FakeSeedWork.GuildUsers.ToList());
            return Task.FromResult(collection);
        }

        /// <inheritdoc />
        public Task<IGuildUser> GetUserAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null) => 
            Task.FromResult(FakeSeedWork.GuildUsers.SingleOrDefault(x => x.Id == id));

        /// <inheritdoc />
        public Task<IGuildUser> GetCurrentUserAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IGuildUser> GetOwnerAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task DownloadUsersAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<int> PruneUsersAsync(int days = 30, bool simulate = false, RequestOptions options = null,
            IEnumerable<ulong> includeRoleIds = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IReadOnlyCollection<IGuildUser>> SearchUsersAsync(string query, int limit = 1000, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IReadOnlyCollection<IAuditLogEntry>> GetAuditLogsAsync(int limit = 100, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null,
            ulong? beforeId = null, ulong? userId = null, ActionType? actionType = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IWebhook> GetWebhookAsync(ulong id, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IReadOnlyCollection<IWebhook>> GetWebhooksAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<GuildEmote> GetEmoteAsync(ulong id, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<GuildEmote> CreateEmoteAsync(string name, Image image, Optional<IEnumerable<IRole>> roles = new Optional<IEnumerable<IRole>>(), RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<GuildEmote> ModifyEmoteAsync(GuildEmote emote, Action<EmoteProperties> func, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task DeleteEmoteAsync(GuildEmote emote, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public int AFKTimeout { get; set; }

        /// <inheritdoc />
        public bool IsEmbeddable { get; set; }

        /// <inheritdoc />
        public bool IsWidgetEnabled { get; set; }

        /// <inheritdoc />
        public DefaultMessageNotifications DefaultMessageNotifications { get; set; }

        /// <inheritdoc />
        public MfaLevel MfaLevel { get; set; }

        /// <inheritdoc />
        public VerificationLevel VerificationLevel { get; set; }

        /// <inheritdoc />
        public ExplicitContentFilterLevel ExplicitContentFilter { get; set; }

        /// <inheritdoc />
        public string IconId { get; set; }

        /// <inheritdoc />
        public string IconUrl { get; set; }

        /// <inheritdoc />
        public string SplashId { get; set; }

        /// <inheritdoc />
        public string SplashUrl { get; set; }

        /// <inheritdoc />
        public string DiscoverySplashId { get; set; }

        /// <inheritdoc />
        public string DiscoverySplashUrl { get; set; }

        /// <inheritdoc />
        public bool Available { get; set; }

        /// <inheritdoc />
        public ulong? AFKChannelId { get; set; }

        /// <inheritdoc />
        public ulong DefaultChannelId { get; set; }

        /// <inheritdoc />
        public ulong? EmbedChannelId { get; set; }

        /// <inheritdoc />
        public ulong? WidgetChannelId { get; set; }

        /// <inheritdoc />
        public ulong? SystemChannelId { get; set; }

        /// <inheritdoc />
        public ulong? RulesChannelId { get; set; }

        /// <inheritdoc />
        public ulong? PublicUpdatesChannelId { get; set; }

        /// <inheritdoc />
        public ulong OwnerId { get; set; }

        /// <inheritdoc />
        public ulong? ApplicationId { get; set; }

        /// <inheritdoc />
        public string VoiceRegionId { get; set; }

        /// <inheritdoc />
        public IAudioClient AudioClient { get; set; }

        /// <inheritdoc />
        public IRole EveryoneRole { get; set; }

        /// <inheritdoc />
        public IReadOnlyCollection<GuildEmote> Emotes { get; set; }

        /// <inheritdoc />
        public IReadOnlyCollection<string> Features { get; set; }

        /// <inheritdoc />
        public IReadOnlyCollection<IRole> Roles { get; set; }

        /// <inheritdoc />
        public PremiumTier PremiumTier { get; set; }

        /// <inheritdoc />
        public string BannerId { get; set; }

        /// <inheritdoc />
        public string BannerUrl { get; set; }

        /// <inheritdoc />
        public string VanityURLCode { get; set; }

        /// <inheritdoc />
        public SystemChannelMessageDeny SystemChannelFlags { get; set; }

        /// <inheritdoc />
        public string Description { get; set; }

        /// <inheritdoc />
        public int PremiumSubscriptionCount { get; set; }

        /// <inheritdoc />
        public int? MaxPresences { get; set; }

        /// <inheritdoc />
        public int? MaxMembers { get; set; }

        /// <inheritdoc />
        public int? MaxVideoChannelUsers { get; set; }

        /// <inheritdoc />
        public int? ApproximateMemberCount { get; set; }

        /// <inheritdoc />
        public int? ApproximatePresenceCount { get; set; }

        /// <inheritdoc />
        public string PreferredLocale { get; set; }

        /// <inheritdoc />
        public CultureInfo PreferredCulture { get; set; }

        #endregion
    }
}