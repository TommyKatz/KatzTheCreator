using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatzTheCreator.UserModules
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command("help")]

        public async Task PingAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.AddField("NeaBot Command Help", "created by @katz#9999")
                .AddField("Commands",
                "**help** | Brings you here" +
                "\n**serverinfo** | See info on the current server" +
                "\n**userinfo** | See info on a certain user" +
                "\n**ping** | Checks the delay between you and the bot" +
                "\n**bean** | Bean a member" +
                "\n**neko** | Post a random neko pic (you need attach files perm in that channel)")
                .AddField("Staff Commands",
                "\n**ban** | Ban a member (requires server ban permission)" +
                "\n**purge** | Purge x messages, where x is 2-100" +
                "\n**role add** | usage: n!role add @user role (put role in quotations)" +
                "\n**role remove** | usage: n!role remove @user role (put role in quotations)")
                .AddField("NSFW Commands",
                "\n**lewd** | Post a random NSFW Neko (requires NSFW channel)" +
                "\n**pussy** | Post a random pussy pic (requires NSFW channel)" +
                "\n**boobs** | Post a random boob pic (requires NSFW channel)" +
                "\n**nekogif** | Post a random NSFW Neko Gif (requires NSFW channel)" +
                "\n**lesbian** | Post a random NSFW lesbian pic (requires NSFW channel)")
                .WithColor(Color.Magenta)
                .WithThumbnailUrl(Context.Guild.IconUrl);

            await ReplyAsync("", false, builder.Build());
            await Context.Message.DeleteAsync();
        }
    }
}

