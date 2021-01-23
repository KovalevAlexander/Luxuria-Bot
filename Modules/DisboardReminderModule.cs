using System.Threading.Tasks;

using Discord.Commands;

using LuxuriaBot.Services;

namespace LuxuriaBot.Modules
{
    public class DisboardReminderModule : ModuleBase<SocketCommandContext>
    {
        readonly DisboardReminderService _service;

        public DisboardReminderModule(DisboardReminderService service) => _service = service;

        [Command("d bump")]
        [RequireContext(ContextType.Guild)]
        public async Task StartTimer()
        {
            if (Context.Channel.Id == _service.Channel.Id)
                await _service.StartTimer().ConfigureAwait(false);
        }

        [Command("setBumpChannel")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task SetChannelAsync(ulong id = 0)
        {
            if (id == 0)
                id = Context.Channel.Id;

            await _service.UpdateChannel(id).ConfigureAwait(false);

            await ReplyAsync($"New bump channel is <#{id}>").ConfigureAwait(false);
        }

        [Command("stopDisboardTimer")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task StopTimer()
        {
            await _service.StopTimer().ConfigureAwait(false);

            await ReplyAsync($"The timer has been stopped!").ConfigureAwait(false);
        }

        [Command("setDisboardReminderMessage")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task SetNewUserMessage(string text)
        {
            if (text == null)
                await Task.CompletedTask.ConfigureAwait(false);

            await _service.UpdateReminderMessage(text).ConfigureAwait(false);

            await ReplyAsync($"Now its set as:\n{text}").ConfigureAwait(false);
        }
    }
}
