using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Bogus;
using Discord;

namespace DiscordChannelsOnDemand.Tests.SeedWork
{
    public sealed class GuildUserStub : IGuildUser
    {
        #region Implementation of IEntity<ulong>

        /// <inheritdoc />
        public ulong Id { get; set; }

        #endregion

        #region Implementation of ISnowflakeEntity

        /// <inheritdoc />
        public DateTimeOffset CreatedAt { get; set; }

        #endregion

        #region Implementation of IMentionable

        /// <inheritdoc />
        public string Mention { get; set; }

        #endregion

        #region Implementation of IPresence

        /// <inheritdoc />
        public IActivity Activity { get; set; }

        /// <inheritdoc />
        public UserStatus Status { get; set; }

        /// <inheritdoc />
        public IImmutableSet<ClientType> ActiveClients { get; set; }

        /// <inheritdoc />
        public IImmutableList<IActivity> Activities { get; set; }

        #endregion

        #region Implementation of IUser

        /// <inheritdoc />
        public string GetAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string GetDefaultAvatarUrl()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IDMChannel> GetOrCreateDMChannelAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string AvatarId { get; set; }

        /// <inheritdoc />
        public string Discriminator { get; set; }

        /// <inheritdoc />
        public ushort DiscriminatorValue { get; set; }

        /// <inheritdoc />
        public bool IsBot { get; set; }

        /// <inheritdoc />
        public bool IsWebhook { get; set; }

        /// <inheritdoc />
        public string Username { get; set; }

        /// <inheritdoc />
        public UserProperties? PublicFlags { get; set; }

        #endregion

        #region Implementation of IVoiceState

        /// <inheritdoc />
        public bool IsDeafened { get; set; }

        /// <inheritdoc />
        public bool IsMuted { get; set; }

        /// <inheritdoc />
        public bool IsSelfDeafened { get; set; }

        /// <inheritdoc />
        public bool IsSelfMuted { get; set; }

        /// <inheritdoc />
        public bool IsSuppressed { get; set; }

        /// <inheritdoc />
        public IVoiceChannel VoiceChannel { get; set; }

        /// <inheritdoc />
        public string VoiceSessionId { get; set; }

        /// <inheritdoc />
        public bool IsStreaming { get; set; }

        #endregion

        #region Implementation of IGuildUser

        /// <inheritdoc />
        public ChannelPermissions GetPermissions(IGuildChannel channel)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task KickAsync(string reason = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task ModifyAsync(Action<GuildUserProperties> func, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task AddRoleAsync(IRole role, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task AddRolesAsync(IEnumerable<IRole> roles, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task RemoveRoleAsync(IRole role, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task RemoveRolesAsync(IEnumerable<IRole> roles, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public DateTimeOffset? JoinedAt { get; set; }

        /// <inheritdoc />
        public string Nickname { get; set; }

        /// <inheritdoc />
        public GuildPermissions GuildPermissions { get; set; }

        /// <inheritdoc />
        public IGuild Guild { get; set; }

        /// <inheritdoc />
        public ulong GuildId { get; set; }

        /// <inheritdoc />
        public DateTimeOffset? PremiumSince { get; set; }

        /// <inheritdoc />
        public IReadOnlyCollection<ulong> RoleIds { get; set; }

        /// <inheritdoc />
        public bool? IsPending { get; set; }

        #endregion
    }
}