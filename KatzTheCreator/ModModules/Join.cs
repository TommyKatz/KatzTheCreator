using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.ModModules
{
    public class Join : ModuleBase<SocketCommandContext>{

        [Command("join")]
        public async Task JoinUser(SocketGuildUser userToBeJoined = null){

            var rUser = Context.User as SocketGuildUser;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var modRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965726434624688188);


            if (rUser.Roles.Contains(directorRole) || rUser.Roles.Contains(modRole)){

                if (userToBeJoined == null){
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nYou didn't specify a user; Identify them using their ``Discord ID`` or ``@Mention``.");
                    return;
                } else if (userToBeJoined.VoiceChannel == null){
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nThe user you are trying to join is not in a voice channel.");
                    return;
                } else if (rUser.VoiceChannel == null){
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nYou are not in a voice channel; Join one to use this.");
                    return;
                } else {
                    var userToJoinVC = userToBeJoined.VoiceChannel.Id;
                    await rUser.ModifyAsync(x => { x.ChannelId = userToJoinVC; });
                    await Context.Message.DeleteAsync();
                }

            } else {
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou do not have permission to use this.");
            }
        }
    }
}
