using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatzTheCreator.OwnerModules
{
    public class GiveawaySelector : ModuleBase<SocketCommandContext>
    {
        [Command("gaw")]
        [RequireOwner]
        public async Task GrabbingEmbedReactions(ulong msg)
        {
            var grabbedMessage = Context.Channel.GetMessageAsync(msg).Result;
            Emote joinEmote = Emote.Parse("<a:join:993953953832251542>");

            await Context.Message.DeleteAsync();
            // grab reaction users
            IEnumerable<IUser> userList = await grabbedMessage.GetReactionUsersAsync(joinEmote, 100).FlattenAsync();
            // filter out bot reactions
            var checkedUsers = userList.Where(u => !u.IsBot);
            // randomize and select winner
            var rnd = new Random();
            var randomizeUsers = checkedUsers.OrderBy(u => rnd.Next(0, checkedUsers.Count()));
            var selectedWinner = randomizeUsers.FirstOrDefault();

            var embedBuilder = new EmbedBuilder()
                .WithColor(Color.Blue)
                .WithAuthor("Winner Winner Chicken Dinner !", $"{selectedWinner.GetAvatarUrl()}")
                .WithDescription($"{selectedWinner.Mention} is the winner of this giveaway !");
            Embed embed = embedBuilder.Build();

            await ReplyAsync(embed: embed);
            //await grabbedMessage.DeleteAsync();
        }
    }
}
