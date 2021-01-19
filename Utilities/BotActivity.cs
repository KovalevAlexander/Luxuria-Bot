using Discord;

namespace LuxuriaBot.Utilities
{
    public class BotActivity : IActivity
    {
        public BotActivity(string name)
        {
            Name = name;
            Details = name;
        }

        public string Name { get; }
        public ActivityType Type { get; } = ActivityType.Playing;
        public ActivityProperties Flags { get; } = ActivityProperties.None;
        public string Details { get; }
    }
}
