using System;
using System.Net.WebSockets;
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
            client.Log += LogAsync;
            client.Ready += LogReady;

            command.Log += LogAsync;
            command.CommandExecuted += LogCommandExecutedAsync;
        }

        public Task LogAsync(LogMessage message)
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
                case System.Exception exception:
                    if ((uint)exception.HResult == 0x80004005)
                        Console.WriteLine($"[{message.Severity}] {CurrentTime} Discord closed the connection");
                    else
                        Console.WriteLine($"[{message.Severity}] {CurrentTime} Unhandled Exception");
                    break;
                default:
                    Console.WriteLine($"[{message.Severity}] {message.ToString()}");
                    break;
            }

            return Task.CompletedTask;
        }

        Task LogCommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
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
