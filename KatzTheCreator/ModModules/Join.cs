using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RequireUserPermissionAttribute = Discord.Commands.RequireUserPermissionAttribute;

namespace KatzTheCreator.ModModules{
    public class Join : ModuleBase<SocketCommandContext>{
        [Command("join")]
        [RequireUserPermission(GuildPermission.MoveMembers)]
        public async Task JoinUser(SocketGuildUser userToBeJoined = null){
            var rUser = Context.User as SocketGuildUser;

            if (userToBeJoined == null){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou didn't specify a user; Identify them using their ``Discord ID`` or ``@Mention``.");
                return;
            }else if (userToBeJoined.VoiceChannel == null){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nThe user you are trying to join is not in a voice channel.");
                return;
            }else if (rUser.VoiceChannel == null){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou are not in a voice channel; Join one to use this.");
                return;
            }else{
                var userToJoinVC = userToBeJoined.VoiceChannel.Id;
                await rUser.ModifyAsync(x => { x.ChannelId = userToJoinVC; });
                await Context.Message.DeleteAsync();
            }
        }
    }
}
