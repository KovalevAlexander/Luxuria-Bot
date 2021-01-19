using System.Threading.Tasks;

using Discord.Commands;

using LuxuriaBot.Services;

namespace LuxuriaBot.Modules
{
    public class UserWatcherModule : ModuleBase<SocketCommandContext>
    {
        readonly UserWatcherService _service;

        public UserWatcherModule(UserWatcherService service) 
            => _service = service;

        [Command("setGreetingsChannel")]
        [Alias("SetGreetingsChannel", "setHelloChannel", "SetHelloChannel")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task SetChannelAsync(ulong id = 0)
        {
            if (id == 0)
                id = Context.Channel.Id;

            _service.UpdateChannel(id.ToString());


            await ReplyAsync($"New UserWatcher Channel is <#{id}>");

            await Task.CompletedTask;
        }

        [Command("setNewUserMessage")]
        [Alias("setGreetingsMessage","setHelloMessage")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task SetNewUserMessage(string text)
        {
            if (text == null)
                await Task.CompletedTask;

            _service.UpdateNewUserMessage(text);
            await ReplyAsync($"Now its set as:\n{text}");
        }

        [Command("setUserLeftMessage")]
        [Alias("setGoodbyeMessage", "setFarewellMessage")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task SetUserLeftMessage(string text)
        {
            if (text == null)
                await Task.CompletedTask;

            _service.UpdateUserLeftMessage(text);
            await ReplyAsync($"Now its set as:\n{text}");
        }
    }
}
