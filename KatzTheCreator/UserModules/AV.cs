using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.UserModules{
    public class AV : ModuleBase<SocketCommandContext>{
        [Command("av")]
        public async Task AVUser (SocketGuildUser userToBeAved = null){

            var rUser = Context.User as SocketGuildUser;

            if (userToBeAved == null){

                var embedBuilder = new EmbedBuilder()
                    .WithAuthor(rUser)
                    .WithColor(Color.DarkPurple)
                    .WithTitle("Avatar")
                    .WithImageUrl(rUser.GetAvatarUrl(ImageFormat.Auto, 320))
                    .WithFooter($"Requested by {rUser}");

                Embed embed = embedBuilder.Build();
                await ReplyAsync(embed: embed);
                await Context.Message.DeleteAsync();

            } else {

                var embedBuilder = new EmbedBuilder()
                    .WithAuthor(userToBeAved)
                    .WithColor(Color.DarkPurple)
                    .WithTitle("Avatar")
                    .WithImageUrl(userToBeAved.GetAvatarUrl(ImageFormat.Auto, 320))
                    .WithFooter($"Requested by {rUser}");

                Embed embed = embedBuilder.Build();
                await ReplyAsync(embed: embed);
                await Context.Message.DeleteAsync();
            }

            
        }
    }
}
