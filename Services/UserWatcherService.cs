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
            SetUserWatcherChannel(_config.Config["UserWatcherChannel"]);
            _newUserMessage = _config.Config["NewUserMessage"];
            _userLeftMessage = _config.Config["UserLeftMessage"];

            return Task.CompletedTask;
        }

        async Task OnUserJoined(SocketGuildUser user)
        {
            if (_userWatcherChannel != null)
                await SendMessage(_newUserMessage, user.Id.ToString());
            else
                await Task.CompletedTask;
        }

        async Task OnUserLeft(SocketGuildUser user)
        {
            if (_userWatcherChannel != null)
                await SendMessage(_userLeftMessage, user.Id.ToString());
            else
                await Task.CompletedTask;
        }

        async Task SendMessage(string message, string userID) 
            => await _userWatcherChannel?.SendMessageAsync(message.Replace("user",$"<@{userID}>"));

        void SetUserWatcherChannel(string id) 
            => _userWatcherChannel = _client.GetChannel(Convert.ToUInt64(id)) as SocketTextChannel;

        public void UpdateChannel(string id)
        {
            SetUserWatcherChannel(id);
            _config.UpdateUserWatcherChannel(id);
        }

        public void UpdateNewUserMessage(string text)
        {
            _newUserMessage = text;
            _config.UpdateUserWatcherNewUserMessage(text);
        }

        public void UpdateUserLeftMessage(string text)
        {
            _userLeftMessage = text;
            _config.UpdateUserWatcherUserLeftMessage(text);
        }
    }
}
