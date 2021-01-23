using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

namespace LuxuriaBot.Utilities
{
    public class Configuration
    {
        readonly string _configPath = Directory.GetCurrentDirectory() + @"\config.json";
        readonly ConfigModel _model = new ConfigModel();

        public IConfiguration Config { get; private set; }

        public async Task<IConfiguration> BuildConfig()
        {
            
            if (!File.Exists(_configPath))
            {
                _model.Token = AskForToken(_configPath);
                await CreateConfigurationFileAsync(_configPath);
            }

            Config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", false, true)
                .Build();

            _model.Token = Config["Token"];
            _model.CommandPrefix = Config["CommandPrefix"];
            _model.ActivityName = Config["ActivityName"];

            _model.UserWatcherChannel = Config["UserWatcherChannel"];
            _model.NewUserMessage = Config["NewUserMessage"];
            _model.UserLeftMessage = Config["UserLeftMessage"];

            _model.DisboardReminderChannel = Config["DisboardReminderChannel"];
            _model.DisboardReminderMessage = Config["DisboardReminderMessage"];

            return Config;
        }

        async Task CreateConfigurationFileAsync(string path)
        {
            await using var stream = File.Create(path);
            await JsonSerializer.SerializeAsync(stream, (object)_model, new JsonSerializerOptions() { WriteIndented = true });
        }

        string AskForToken(string path)
        {
            Console.WriteLine("No configuration file found. Creating a new one at: " + path);
            Console.WriteLine("Enter the token: ");

            return Console.ReadLine();
        }

        public async Task UpdateUserWatcherChannel(string id)
        {
            _model.UserWatcherChannel = id;

            await CreateConfigurationFileAsync(_configPath);
        }

        public async Task UpdateUserWatcherNewUserMessage(string text)
        {
            _model.NewUserMessage = text;

            await CreateConfigurationFileAsync(_configPath);
        }

        public async Task UpdateUserWatcherUserLeftMessage(string text)
        {
            _model.UserLeftMessage = text;

            await CreateConfigurationFileAsync(_configPath);
        }

        public async Task UpdateDisboardReminderChannel(string id)
        {
            _model.DisboardReminderChannel = id;

            await CreateConfigurationFileAsync(_configPath);
        }

        public async Task UpdateDisboardReminderMessage(string text)
        {
            _model.DisboardReminderMessage = text;

            await CreateConfigurationFileAsync(_configPath);
        }
    }
}
