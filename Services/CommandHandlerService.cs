using System;
using System.Reflection;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.WebSocket;

using LuxuriaBot.Utilities;

namespace LuxuriaBot.Services
{
    public class CommandHandlerService
    {
        readonly DiscordSocketClient _client;
        readonly CommandService _commands;
        readonly IServiceProvider _provider;

        readonly char _commandPrefix;

        public CommandHandlerService(IServiceProvider provider, Configuration config, DiscordSocketClient client, CommandService commands)
        {
            _client = client;
            _commands = commands;
            _provider = provider;

            _commandPrefix = Convert.ToChar(config.Config["CommandPrefix"]);

            _client.MessageReceived += HandleCommandAsync;
        }

        public async Task InitializeAsync() 
            => await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);

        async Task HandleCommandAsync(SocketMessage messageParam)
        {
            if (!(messageParam is SocketUserMessage message) || message.Author.IsBot) return;

            var argPos = 0;

            if (!(message.HasCharPrefix(_commandPrefix, ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
                return;

            var context = new SocketCommandContext(_client, message);
            await _commands.ExecuteAsync(
                context,
                argPos,
                _provider);
        }
    }
}
