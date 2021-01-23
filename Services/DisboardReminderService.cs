using System;
using System.Timers;
using System.Threading.Tasks;

using Discord.WebSocket;

using LuxuriaBot.Utilities;

namespace LuxuriaBot.Services
{
    public class DisboardReminderService
    {
        const int IntervalBetweenReminders = 120;

        readonly DiscordSocketClient _client;
        readonly Configuration _config;

        public SocketTextChannel Channel { get; private set; }

        Timer _timer;
        string _reminderMessage;
        
        public DisboardReminderService(DiscordSocketClient client, Configuration config)
        {
            _client = client;
            _config = config;

            _client.Ready += Initialize;
        }

        async Task Initialize()
        {
            SetDisboardReminderChannel(Convert.ToUInt64(_config.Config["DisboardReminderChannel"]));
            _reminderMessage = _config.Config["DisboardReminderMessage"];

            await Task.CompletedTask.ConfigureAwait(false);
        }

        public async Task StartTimer()
        {
            if (_timer == null)
            {
                _timer = new Timer(IntervalBetweenReminders * 60000) {AutoReset = true};
                _timer.Elapsed += async (s, e) => await Remind(s,e).ConfigureAwait(false);
                _timer.Start();
                
                await SendMessageAsync("Luxy will remind everyone to bump every 2 hours from now on!").ConfigureAwait(false);
            }
        }

        public async Task Remind(object source, ElapsedEventArgs e) 
            => await SendMessageAsync(_reminderMessage).ConfigureAwait(false);

        public async Task StopTimer()
        {
            if (_timer == null)
                await Task.CompletedTask.ConfigureAwait(false);

            _timer = null;
        }

        public async Task UpdateChannel(ulong id)
        {
            SetDisboardReminderChannel(id);
            await _config.UpdateDisboardReminderChannel(id).ConfigureAwait(false);
        }

        public async Task UpdateReminderMessage(string message)
        {
            _reminderMessage = message;
            await _config.UpdateDisboardReminderMessage(message).ConfigureAwait(false);
        }

        async Task SendMessageAsync(string message) 
            => await Channel.SendMessageAsync(message).ConfigureAwait(false);

        void SetDisboardReminderChannel(ulong id) 
            => Channel = _client.GetChannel(id) as SocketTextChannel;
    }
}
