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

            await _service.UpdateChannel(id).ConfigureAwait(false);


            await ReplyAsync($"New UserWatcher Channel is <#{id}>").ConfigureAwait(false);
        }

        [Command("setNewUserMessage")]
        [Alias("setGreetingsMessage","setHelloMessage")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task SetNewUserMessage(string text)
        {
            if (text == null)
                await Task.CompletedTask.ConfigureAwait(false);

            await _service.UpdateNewUserMessage(text).ConfigureAwait(false);

            await ReplyAsync($"Now its set as:\n{text}").ConfigureAwait(false);
        }

        [Command("setUserLeftMessage")]
        [Alias("setGoodbyeMessage", "setFarewellMessage")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task SetUserLeftMessage(string text)
        {
            if (text == null)
                await Task.CompletedTask.ConfigureAwait(false);

            await _service.UpdateUserLeftMessage(text).ConfigureAwait(false);

            await ReplyAsync($"Now its set as:\n{text}").ConfigureAwait(false);
        }
    }
}
