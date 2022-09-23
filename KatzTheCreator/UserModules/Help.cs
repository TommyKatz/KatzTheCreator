using Discord.Commands;
using Discord;

namespace KatzTheCreator.UserModules{
    public class Help : ModuleBase<SocketCommandContext>{
        [Command("help")]
        public async Task PingAsync(){
            EmbedBuilder builder = new EmbedBuilder();
            
            builder.AddField("NeaBot Command Help", "created by @katz#9999")
                .AddField("Commands (no permissions needed)",
                "**help** | *Brings you here*\nusage: ``?help``" +
                "\n**av** | *Enlarges the members pfp*\nusage: ``?av 135143527767080960``" +
                "\n**serverinfo** | *Provides info on the current server*\nusage: ``?serverinfo``" +
                "\n**whois** | *Provides info on a certain user*\nusage: ``?whois 135143527767080960``")
                .AddField("Staff Commands (permission dependent)",
                "\n**ban** | *Bans a user*\nusage: ``?ban 135143527767080960 reason``" +
                "\n**unban** | *Unbans a user*\nusage: ``?unban 135143527767080960 reason``" +
                "\n**revoke** | *Kicks unvaluable members*\nusage: ``?revoke 135143527767080960 reason``" +
                "\n**mute** | *Mutes a member*\nusage: ``?giverole 135143527767080960 roleName``" +
                "\n**unmute** | *Unmutes a member*\nusage: ``?removerole 135143527767080960 roleName``" +
                "\n**giverole** | *Gives a specified role to a member*\nusage: ``?giverole 135143527767080960 roleName``" +
                "\n**removerole** | *Removes a specified role to a member*\nusage: ``?removerole 135143527767080960 roleName``" +
                "\n**bring** | *Drags a vc member to your vc*\nusage: ``?bring 135143527767080960 roleName``" +
                "\n**join** | *Drags you to a vc member*\nusage: ``?join 135143527767080960 roleName``" +
                "\n**clear** | *Clears a specified amount of messages*\nusage: ``?clear 14``" +
                "\n**ping** | *Grabs the websocket's latency*\nusage: ``?ping``")
                .WithColor(Color.DarkPurple)
                .WithThumbnailUrl(Context.Guild.IconUrl);

            Embed embed = builder.Build();

            await ReplyAsync(embed: embed);
            await Context.Message.DeleteAsync();
        }
    }
}

