using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace LuxuriaBot.Services
{
    public class GeneralCommandsService
    {
        readonly CommandService _service;

        public GeneralCommandsService(CommandService service)
        {
            _service = service;
        }

        public async Task<Embed> BuildHelpMessageAsync(ICommandContext context)
        {
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = "These are the commands you can use"
            };

            foreach (var module in _service.Modules)
            {
                string description = null;
                foreach (var cmd in module.Commands)
                {
                    var result = await cmd.CheckPreconditionsAsync(context);
                    if (result.IsSuccess)
                        description += $"{cmd.Aliases.First()}\n";
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    builder.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }
            return builder.Build();
        }
    }
}
