using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

namespace LuxuriaBot.Services
{
    public class ModerationService
    {
        public async Task KickAsync(IReadOnlyCollection<SocketUser> users, string reason = null)
        {
            foreach (var user in users)
                await (user as SocketGuildUser).KickAsync(reason);
        }

        public async Task BanAsync(IReadOnlyCollection<SocketUser> users, string reason = null)
        {
            foreach (var user in users)
                await (user as SocketGuildUser).BanAsync(0, reason);
        }

        public async Task PurgeAsync(SocketTextChannel channel, uint count)
        {
            var messages = await channel.GetMessagesAsync((int)count + 1).FlattenAsync();

            await channel.DeleteMessagesAsync(
                messages.Where(x => (DateTimeOffset.UtcNow - x.Timestamp).TotalDays <= 14));
        }
    }
}
