using System.Text;
using System.Threading.Tasks;

using Discord.Commands;

namespace LuxuriaBot.Modules
{
    public class GeneralCommandsModule : ModuleBase<SocketCommandContext>
    {
        const string PingMessageFormat = "Hi there, <@{0}>!";

        [Command("ping")]
        [Alias("hi", "hello")]
        [RequireContext(ContextType.Guild)]
        public async Task PingAsync()
        {
            var message = new StringBuilder();
            message.AppendFormat(PingMessageFormat, Context.User.Id);

            await ReplyAsync(message.ToString());
        }
    }
}
