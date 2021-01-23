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
        public ulong UserWatcherChannel { get; set; } = 0;
        [JsonInclude]
        public string NewUserMessage { get; set; } = "";
        [JsonInclude]
        public string UserLeftMessage { get; set; } = "";
        [JsonInclude]
        public ulong DisboardReminderChannel { get; set; } = 0;
        [JsonInclude]
        public string DisboardReminderMessage { get; set; } = "";
    }
}
