using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.WebSocket;
using Discord.Commands;

using LuxuriaBot.Utilities;
using LuxuriaBot.Services;
using LuxuriaBot.Modules;

namespace LuxuriaBot
{
    internal class LuxuriaBot
    {

        DiscordSocketClient _client;
        IConfiguration _config;
        CommandHandlerService _cmdHandler;
        LoggingService _logger;
        ReliabilityService _reliabilityService;

        readonly DiscordSocketConfig _socketConfig = new DiscordSocketConfig
        {
            MessageCacheSize = 100,
            ExclusiveBulkDelete = true
        };

        static void Main(string[] args)
            => new LuxuriaBot().MainAsync().GetAwaiter().GetResult();

        async Task MainAsync()
        {
            Console.Title = "LuxuriaBot";

            await InitializeServices().ConfigureAwait(false);

            await _client.LoginAsync(TokenType.Bot, _config["token"]).ConfigureAwait(false);
            await _client.StartAsync().ConfigureAwait(false);

            _client.Ready += async () => await _client.SetGameAsync(_config["ActivityName"]).ConfigureAwait(false);

            await Task.Delay(Timeout.Infinite).ConfigureAwait(false);
        }

        async Task InitializeServices()
        {
            var services = ConfigureServices();

            _config = await services.GetRequiredService<Configuration>().BuildConfig().ConfigureAwait(false);
            _client = services.GetRequiredService<DiscordSocketClient>();
            _cmdHandler = services.GetRequiredService<CommandHandlerService>();
            await _cmdHandler.InitializeAsync().ConfigureAwait(false);

            _logger = services.GetRequiredService<LoggingService>();
            _reliabilityService = services.GetRequiredService<ReliabilityService>();
        }

        public IServiceProvider ConfigureServices()
        {
            var collection = new ServiceCollection()
                .AddSingleton<Configuration>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlerService>()
                .AddSingleton<UserWatcherService>()
                .AddSingleton<UserWatcherModule>()
                .AddSingleton<LoggingService>()
                .AddSingleton<GeneralCommandsService>()
                .AddSingleton<GeneralCommandsModule>()
                .AddSingleton<DisboardReminderService>()
                .AddSingleton<DisboardReminderModule>()
                .AddSingleton<ModerationService>()
                .AddSingleton<ModerationModule>()
                .AddSingleton<ReliabilityService>();

            collection.AddSingleton(delegate { return new DiscordSocketClient(_socketConfig); });
            return collection.BuildServiceProvider();
        }
    }
}
