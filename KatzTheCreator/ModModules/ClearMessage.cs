using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RequireUserPermissionAttribute = Discord.Commands.RequireUserPermissionAttribute;

namespace KatzTheCreator.ModModules{
    public class ClearMessage : ModuleBase<SocketCommandContext>{
        [Command("clear")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ClearMessages(int amount = default){
            var rUser = Context.User as SocketGuildUser;
            var waitTimeFive = 5000;

            if (amount <= 0 || amount == default){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nCommand unsuccessful; Ensure correct format.");
                return;
            }else{
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
        }   
    }
}
