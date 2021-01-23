using System.Text;
using System.Threading.Tasks;

using Discord.Commands;

using LuxuriaBot.Services;

namespace LuxuriaBot.Modules
{
    public class GeneralCommandsModule : ModuleBase<SocketCommandContext>
    {
        readonly GeneralCommandsService _service;

        const string PingMessageFormat = "Hi there, <@{0}>!";

        public GeneralCommandsModule(GeneralCommandsService service)
            =>_service = service;

        [Command("ping")]
        [Alias("hi", "hello")]
        [RequireContext(ContextType.Guild)]
        public async Task PingAsync()
        {
            var message = new StringBuilder();
            message.AppendFormat(PingMessageFormat, Context.User.Id);
            
            await ReplyAsync(message.ToString()).ConfigureAwait(false);
        }

        [Command("help")]
        [RequireContext(ContextType.Guild)]
        public async Task HelpAsync()
        {
            await ReplyAsync("", false, await _service.BuildHelpMessageAsync(Context).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}
