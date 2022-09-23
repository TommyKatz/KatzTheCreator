using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.ModModules{
    public class ClearMessage : ModuleBase<SocketCommandContext>{
        [Command("clear")]
        [Summary("Deletes the specified amount of messages.")]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task ClearMessages(int amount = default){

            var rUser = Context.User as SocketGuildUser;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var modRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Mod");
            var waitTimeFive = 5000;
            var waitTimeTwo = 2000;

            if (rUser.Roles.Contains(directorRole) || rUser.Roles.Contains(modRole)){

                if (amount <= 0 || amount == default){
                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithDescription($"{rUser.Mention}, Command unsuccessful; Ensure correct format.");
                    Embed embed = embedBuilder.Build();
                    var botReply = await ReplyAsync(embed: embed);
                    await Task.Delay(waitTimeFive);
                    await Context.Message.DeleteAsync();
                    await Task.Delay(waitTimeTwo);
                    await botReply.DeleteAsync();
                    return;
                } else {
                    IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
                    await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);

                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithDescription($"{rUser.Mention}, I have deleted {amount} messages for you :)");
                    Embed embed = embedBuilder.Build();
                    var botReplySuccess = await ReplyAsync(embed: embed);
                    await Task.Delay(waitTimeFive);
                    await botReplySuccess.DeleteAsync();
                }

            } else {

                var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.DarkPurple)
                    .WithDescription($"{rUser.Mention}, You do not have permission to use this.");
                Embed embed = embedBuilder.Build();
                var botReplyFailPerms = await ReplyAsync(embed: embed);
                await Task.Delay(waitTimeFive);
                await Context.Message.DeleteAsync();
                await Task.Delay(waitTimeTwo);
                await botReplyFailPerms.DeleteAsync();
            }
        }   
    }
}
