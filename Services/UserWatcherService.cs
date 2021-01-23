using System;
using System.Threading.Tasks;

using Discord.WebSocket;

using LuxuriaBot.Utilities;

namespace LuxuriaBot.Services
{
    public class UserWatcherService
    {
        readonly DiscordSocketClient _client;
        readonly Configuration _config;

        SocketTextChannel _userWatcherChannel;
        string _newUserMessage;
        string _userLeftMessage;

        public UserWatcherService(DiscordSocketClient client, Configuration config)
        {
            _client = client;
            _config = config;

            _client.Ready += Initialize;
            _client.UserLeft += OnUserLeft;
            _client.UserJoined += OnUserJoined;
        }

        Task Initialize()
        {
            SetUserWatcherChannel(Convert.ToUInt64(_config.Config["UserWatcherChannel"]));
            _newUserMessage = _config.Config["NewUserMessage"];
            _userLeftMessage = _config.Config["UserLeftMessage"];

            return Task.CompletedTask;
        }

        async Task OnUserJoined(SocketGuildUser user)
        {
            if (_userWatcherChannel != null)
                await SendMessage(_newUserMessage, user.Id.ToString()).ConfigureAwait(false);
        }

        async Task OnUserLeft(SocketGuildUser user)
        {
            if (_userWatcherChannel != null)
                await SendMessage(_userLeftMessage, user.Id.ToString()).ConfigureAwait(false);
        }

        async Task SendMessage(string message, string userID)
        {
            message = message.Replace("user", $"<@{userID}>");

            await _userWatcherChannel?.SendMessageAsync(message);
        }


        void SetUserWatcherChannel(ulong id) 
            => _userWatcherChannel = _client.GetChannel(Convert.ToUInt64(id)) as SocketTextChannel;

        public async Task UpdateChannel(ulong id)
        {
            SetUserWatcherChannel(id);
            await _config.UpdateUserWatcherChannel(id).ConfigureAwait(false);
        }

        public async Task UpdateNewUserMessage(string text)
        {
            _newUserMessage = text;
            await _config.UpdateUserWatcherNewUserMessage(text).ConfigureAwait(false);
        }

        public async Task UpdateUserLeftMessage(string text)
        {
            _userLeftMessage = text;
            await _config.UpdateUserWatcherUserLeftMessage(text).ConfigureAwait(false);
        }
    }
}
