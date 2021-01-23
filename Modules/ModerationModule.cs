using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using LuxuriaBot.Services;

namespace LuxuriaBot.Modules
{
    public class ModerationModule : ModuleBase<SocketCommandContext>
    {
        readonly ModerationService _service;

        public ModerationModule(ModerationService service)
            => _service = service;

        [Command("purge", RunMode = RunMode.Async)]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task PurgeAsync(uint count) 
            => await _service.PurgeAsync(Context.Channel as SocketTextChannel, count).ConfigureAwait(false);

        [Command("kick")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task KickAsync(string reason = null) 
            => await _service.KickAsync(Context.Message.MentionedUsers, reason).ConfigureAwait(false);

        [Command("ban")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanAsync(string reason = null)
            => await _service.BanAsync(Context.Message.MentionedUsers, reason).ConfigureAwait(false);
    }
}
