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
            var waitTimeFive = 5000;
            var waitTimeTwo = 2000;

            if (rUser.Roles.Contains(directorRole)){

                if (amount <= 0 || amount == default){

                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nCommand unsuccessful; Ensure correct format.");
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
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou do not have permission to use this.");
            }
        }   
    }
}
