using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System.Linq;

namespace KatzTheCreator.ModModules{
    public class LockandUnlock : ModuleBase<SocketCommandContext>{
        [Command("lock")]
        public async Task LockCurrentChannel(){

            var rUser = Context.User as SocketGuildUser;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var everyoneRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 960957925143224340);
            var channelToLock = Context.Channel as IGuildChannel;
            var lockEmoji = new Emoji("🔒");
            var waitTimeSeven = 7000;
            var newOverwrites = new OverwritePermissions(sendMessages: PermValue.Deny);

            if (rUser.Roles.Contains(directorRole)){

                await Context.Message.DeleteAsync();
                await channelToLock.AddPermissionOverwriteAsync(everyoneRole, newOverwrites);

                var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.DarkPurple)
                    .WithDescription($"{rUser.Mention}, this channel has been locked. {lockEmoji}");
                Embed embed = embedBuilder.Build();
                var botReplySuccess = await ReplyAsync(embed: embed);

            } else {
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou do not have permission to use this.");
            }
        }

        [Command("unlock")]
        public async Task UnlockCurrentChannel(){
            var rUser = Context.User as SocketGuildUser;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var everyoneRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 960957925143224340);
            var channelToUnlock = Context.Channel as IGuildChannel;
            var unlockEmoji = new Emoji("🔓");
            var waitTimeSeven = 7000;
            var newOverwrites = new OverwritePermissions(sendMessages: PermValue.Allow);

            if (rUser.Roles.Contains(directorRole)){

                await Context.Message.DeleteAsync();
                await channelToUnlock.AddPermissionOverwriteAsync(everyoneRole, newOverwrites);

                var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.DarkPurple)
                    .WithDescription($"{rUser.Mention}, this channel has been unlocked. {unlockEmoji}");
                Embed embed = embedBuilder.Build();
                var botReplySuccess = await ReplyAsync(embed: embed);

            } else {
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou do not have permission to use this.");
            }
        }
    }
}
