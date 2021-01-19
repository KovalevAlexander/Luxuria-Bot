using System;
using System.Threading;
using System.Threading.Tasks;

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
        static void Main(string[] args)
            => new LuxuriaBot().MainAsync().GetAwaiter().GetResult();

        async Task MainAsync()
        {
            Console.Title = "LuxuriaBot";

            var clientConfig = new DiscordSocketConfig
            {
                MessageCacheSize = 100, 
                ExclusiveBulkDelete = true
            };

            var services = ConfigureServices(clientConfig);
            var config = services.GetRequiredService<Configuration>().BuildConfig();

            var client = services.GetRequiredService<DiscordSocketClient>();
            await services.GetRequiredService<CommandHandlerService>().InitializeAsync(services);

            InitializeMiscServices(services);

            await client.LoginAsync(TokenType.Bot, config["token"]);
            await client.StartAsync();

            await client.SetActivityAsync(new BotActivity(config["NameOfActivity"]));

            await Task.Delay(Timeout.Infinite);
        }

        void InitializeMiscServices(IServiceProvider service)
        {
            service.GetRequiredService<LoggingService>();
            service.GetRequiredService<ReliabilityService>();
        }

        public IServiceProvider ConfigureServices(DiscordSocketConfig socketConfig)
        {
            var collection = new ServiceCollection()
                .AddSingleton<Configuration>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlerService>()
                .AddSingleton<UserWatcherService>()
                .AddSingleton<UserWatcherModule>()
                .AddSingleton<LoggingService>()
                .AddSingleton<GeneralCommandsModule>()
                .AddSingleton<DisboardReminderService>()
                .AddSingleton<DisboardReminderModule>()
                .AddSingleton<ModerationService>()
                .AddSingleton<ModerationModule>()
                .AddSingleton<ReliabilityService>();

            collection.AddSingleton(delegate { return new DiscordSocketClient(socketConfig); });
            return collection.BuildServiceProvider();
        }
    }
}
