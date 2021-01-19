using System.Text.Json.Serialization;

namespace LuxuriaBot.Utilities
{
    public class ConfigModel
    {
        [JsonInclude]
        public string Token { get; set; } = "";
        [JsonInclude]
        public string CommandPrefix { get; set; } = "!";
        [JsonInclude]
        public string ActivityName { get; set; } = "Lewding";
        [JsonInclude]
        public string UserWatcherChannel { get; set; } = "";
        [JsonInclude]
        public string NewUserMessage { get; set; } = "";
        [JsonInclude]
        public string UserLeftMessage { get; set; } = "";
        [JsonInclude]
        public string DisboardReminderChannel { get; set; } = "";
        [JsonInclude]
        public string DisboardReminderMessage { get; set; } = "";
    }
}
