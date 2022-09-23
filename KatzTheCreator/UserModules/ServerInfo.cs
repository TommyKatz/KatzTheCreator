using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatzTheCreator.UserModules
{
    public class ServerInfo : ModuleBase<SocketCommandContext>
    {
        [Command("serverinfo")]
        public async Task PingAsync()
        {


            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"Server: {Context.Guild.Name}")
                .WithColor(Color.Magenta)
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .AddField("Owner", $"{Context.Guild.Owner}")
                .AddField("Date Created", $"{Context.Guild.CreatedAt.UtcDateTime.ToString("D")}")
                .AddField("Members", $"{Context.Guild.MemberCount}")
                .AddField("Roles", $"{Context.Guild.Roles.Count}")
                .AddField("Text Channels", $"{Context.Guild.TextChannels.Count}")
                .AddField("Voice Channels", $"{Context.Guild.VoiceChannels.Count}")
                .WithFooter($"{DateTime.Now}");

            await ReplyAsync("", false, builder.Build());
        }
    }
}
