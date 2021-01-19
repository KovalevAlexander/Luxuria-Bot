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

        Task Initialize()
        {
            SetDisboardReminderChannel(_config.Config["DisboardReminderChannel"]);
            _reminderMessage = _config.Config["DisboardReminderMessage"];

            return Task.CompletedTask;
        }

        public async Task StartTimer()
        {
            if (_timer == null)
            {
                _timer = new Timer(IntervalBetweenReminders * 60000) {AutoReset = true};
                _timer.Elapsed += Remind;
                _timer.Start();

                await SendMessage("Luxy will remind everyone to bump every 2 hours from now on!");
            }
            await Task.CompletedTask;
        }

        public async void Remind(object source, ElapsedEventArgs e) 
            => await SendMessage(_reminderMessage);

        public async Task StopTimer()
        {
            if (_timer == null)
                await Task.CompletedTask;

            _timer.Elapsed -= Remind;
            _timer = null;
        }

        public void UpdateChannel(string id)
        {
            SetDisboardReminderChannel(id);
            _config.UpdateDisboardReminderChannel(id);
        }

        public void UpdateReminderMessage(string message)
        {
            _reminderMessage = message;
            _config.UpdateDisboardReminderMessage(message);
        }

        async Task SendMessage(string message) 
            => await Channel.SendMessageAsync(message);

        void SetDisboardReminderChannel(string id) 
            => Channel = _client.GetChannel(Convert.ToUInt64(id)) as SocketTextChannel;
    }
}
