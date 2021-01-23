using System;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace LuxuriaBot.Services
{
    public class LoggingService
    {
        static string CurrentTime => DateTime.Now.ToString("HH:mm:ss");

        public LoggingService(DiscordSocketClient client, CommandService command)
        {
            client.Log += Log;
            client.Ready += LogReady;

            command.Log += Log;
            command.CommandExecuted += LogCommandExecuted;
        }

        public Task Log(LogMessage message)
        {
            switch (message.Exception)
            {
                case CommandException cmdException:
                    Console.WriteLine($"[Command/{message.Severity}] {cmdException.Command.Name}"
                                      + $" failed to execute in {cmdException.Context.Channel}.");
                    break;
                case GatewayReconnectException grException:
                    Console.WriteLine($"[{message.Severity}] {CurrentTime} Discord requested to reconnect");
                    break;
                default:
                    Console.WriteLine($"[{message.Severity}] {message.ToString()}");
                    break;
            }
            return Task.CompletedTask;
        }

        Task LogCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!(context.Message is SocketUserMessage)) return Task.CompletedTask;

            var message = context.Message;
            var timeStamp = message.Timestamp.ToLocalTime().DateTime.ToString("HH:mm:ss");
            Console.WriteLine($"[Info] {timeStamp} Command Was Executed: {message.Content} by {message.Author} in {message.Channel.Name}");

            return Task.CompletedTask;
        }

        Task LogReady()
        {
            Console.WriteLine($"[Info] {CurrentTime} Luxuria is up and running!");

            return Task.CompletedTask;
        }
    }
}
