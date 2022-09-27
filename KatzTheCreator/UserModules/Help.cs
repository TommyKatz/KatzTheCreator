using Discord.Commands;
using Discord;

namespace KatzTheCreator.UserModules{
    public class Help : ModuleBase<SocketCommandContext>{
        [Command("help")]
        public async Task PingAsync(){
            EmbedBuilder builder = new EmbedBuilder();
            
            builder.AddField("- NeaBot Command Help -", "• *created by @katz#9999* •")
                .AddField("Commands (no permissions needed)",
                "**help** | *Brings you here*\nusage: ``?help``" +
                "\n**av** | *Enlarges the members pfp*\nusage: ``?av 135143527767080960``" +
                "\n**serverinfo** | *Provides info on the current server*\nusage: ``?serverinfo``" +
                "\n**whois** | *Provides info on a certain user*\nusage: ``?whois 135143527767080960``")
                .AddField("Staff Commands (permission dependent)",
                $"\n*For staff related help, please visit the guide channel or contact a supervisor.*")
                .WithColor(Color.DarkPurple)
                .WithThumbnailUrl(Context.Guild.IconUrl);

            Embed embed = builder.Build();

            await ReplyAsync(embed: embed);
            await Context.Message.DeleteAsync();
        }
    }
}

