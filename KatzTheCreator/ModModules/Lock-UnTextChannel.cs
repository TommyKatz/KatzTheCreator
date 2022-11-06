using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RequireUserPermissionAttribute = Discord.Commands.RequireUserPermissionAttribute;

namespace KatzTheCreator.ModModules{
    public class LockandUnlock : ModuleBase<SocketCommandContext>{
        [Command("lock")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task LockCurrentChannel(){

            var rUser = Context.User as SocketGuildUser;
            var everyoneRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 960957925143224340);
            var channelToLock = Context.Channel as IGuildChannel;
            var lockEmoji = new Emoji("🔒");
            var newOverwrites = new OverwritePermissions(sendMessages: PermValue.Deny);

            await Context.Message.DeleteAsync();
            await channelToLock.AddPermissionOverwriteAsync(everyoneRole, newOverwrites);

            var embedBuilder = new EmbedBuilder()
                .WithColor(Color.DarkPurple)
                .WithDescription($"{rUser.Mention}, this channel has been locked. {lockEmoji}");
            Embed embed = embedBuilder.Build();
            var botReplySuccess = await ReplyAsync(embed: embed);
        }

        [Command("unlock")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task UnlockCurrentChannel(){
            var rUser = Context.User as SocketGuildUser;
            var everyoneRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 960957925143224340);
            var channelToUnlock = Context.Channel as IGuildChannel;
            var unlockEmoji = new Emoji("🔓");
            var newOverwrites = new OverwritePermissions(sendMessages: PermValue.Inherit);

            await Context.Message.DeleteAsync();
            await channelToUnlock.AddPermissionOverwriteAsync(everyoneRole, newOverwrites);

            var embedBuilder = new EmbedBuilder()
                .WithColor(Color.DarkPurple)
                .WithDescription($"{rUser.Mention}, this channel has been unlocked. {unlockEmoji}");
            Embed embed = embedBuilder.Build();
            var botReplySuccess = await ReplyAsync(embed: embed);
        }
    }
}
