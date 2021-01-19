using System;
using System.Threading;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

namespace LuxuriaBot.Services
{
    public class ReliabilityService
    {
        // --- Begin Configuration Section ---
        // How long should we wait on the client to reconnect before resetting?
        static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);

        // Should we attempt to reset the client? Set this to false if your client is still locking up.
        const bool AttemptReset = true;

        // Change log levels if desired:
        const LogSeverity Debug = LogSeverity.Debug;
        const LogSeverity Info = LogSeverity.Info;
        const LogSeverity Critical = LogSeverity.Critical;
        // --- End Configuration Section ---

        readonly DiscordSocketClient _discord;
        readonly LoggingService _logger;
        CancellationTokenSource _cts;

        public ReliabilityService(DiscordSocketClient discord, LoggingService logger = null)
        {
            _cts = new CancellationTokenSource();
            _discord = discord;
            _logger = logger;

            _discord.Connected += ConnectedAsync;
            _discord.Disconnected += DisconnectedAsync;
        }

        public Task ConnectedAsync()
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();

            return Task.CompletedTask;
        }

        public Task DisconnectedAsync(Exception _e)
        {
            // Check the state after <timeout> to see if we reconnected
            _ = InfoAsync("Client disconnected, starting timeout task...");
            _ = Task.Delay(Timeout, _cts.Token).ContinueWith(async _ =>
            {
                await DebugAsync("Timeout expired, continuing to check the client's state...");
                await CheckStateAsync();
                await DebugAsync("State came back okay");
            });

            return Task.CompletedTask;
        }

        async Task CheckStateAsync()
        {
            // Client reconnected, no need to reset
            if (_discord.ConnectionState == ConnectionState.Connected) return;
            if (AttemptReset)
            {
                await InfoAsync("Attempting to reset the client");

                var timeout = Task.Delay(Timeout);
                var connect = _discord.StartAsync();
                var task = await Task.WhenAny(timeout, connect);

                if (task == timeout)
                {
                    await CriticalAsync("Client reset timed out (task deadlocked?), killing the process");
                    FailFast();
                }
                else if (connect.IsFaulted)
                {
                    await CriticalAsync("Client reset failed, killing the process", connect.Exception);
                    FailFast();
                }
                else if (connect.IsCompletedSuccessfully)
                    await InfoAsync("Client has been reset successfully!");
                return;
            }

            await CriticalAsync("Client did not reconnected in time, killing the process");
            FailFast();
        }

        void FailFast()
            => Environment.Exit(1);

        // Logging Helpers
        const string LogSource = "Reliability";
        Task DebugAsync(string message)
            => _logger.LogAsync(new LogMessage(Debug, LogSource, message));
        Task InfoAsync(string message)
            => _logger.LogAsync(new LogMessage(Info, LogSource, message));
        Task CriticalAsync(string message, Exception error = null)
            => _logger.LogAsync(new LogMessage(Critical, LogSource, message, error));
    }
}