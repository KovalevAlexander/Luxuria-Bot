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
            {
                await _service.StartTimer();
            }
            await Task.CompletedTask;
        }

        [Command("setBumpChannel")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task SetChannelAsync(ulong id = 0)
        {
            if (id == 0)
                id = Context.Channel.Id;

            _service.UpdateChannel(id.ToString());


            await ReplyAsync($"New bump channel is <#{id}>");

            await Task.CompletedTask;
        }

        [Command("stopDisboardTimer")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task StopTimer()
        {
            await _service.StopTimer();

            await ReplyAsync($"The timer has been stopped!");

            await Task.CompletedTask;
        }

        [Command("setDisboardReminderMessage")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task SetNewUserMessage(string text)
        {
            if (text != null)
                _service.UpdateReminderMessage(text);

            await ReplyAsync($"Now its set as:\n{text}");

            await Task.CompletedTask;
        }
    }
}
