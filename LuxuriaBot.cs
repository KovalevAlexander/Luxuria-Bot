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

            await InitializeServices();

            await _client.LoginAsync(TokenType.Bot, _config["token"]);
            await _client.StartAsync();

            await _client.SetActivityAsync(new BotActivity(_config["NameOfActivity"]));

            await Task.Delay(Timeout.Infinite);
        }

        async Task InitializeServices()
        {
            var services = ConfigureServices();

            _config = await services.GetRequiredService<Configuration>().BuildConfig();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _cmdHandler = services.GetRequiredService<CommandHandlerService>();
            await _cmdHandler.InitializeAsync();

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
