/*using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.PVCModules{
    public class Unban : ModuleBase<SocketCommandContext>{
        [Command("iunban")]
        public async Task UnbanVCUser(SocketGuildUser user = null){

            var rUser = Context.User as SocketGuildUser;
            var permHasValue = rUser.VoiceChannel.GetPermissionOverwrite(rUser).HasValue;

            if (!permHasValue){
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou must have permission to use this; Create your own PVC or ask for auth from PVC owner.");
                return;
            }

            var permCheck = rUser.VoiceChannel.GetPermissionOverwrite(rUser).Value.Connect;
            var userPermHasValue = rUser.VoiceChannel.GetPermissionOverwrite(user).HasValue;

            if (userPermHasValue){
                var userPermCritera = rUser.VoiceChannel.GetPermissionOverwrite(user).Value.Connect;
                if (userPermCritera == 0){
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nYou don't have permission to use this; This member has elevated permissions in this PVC.");
                    return;
                }
            }

            if (!userPermHasValue){
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nThis member is not banned in this PVC.");
                return;
            }

            if (permCheck != 0){
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou must have permission to use this; Create your own PVC or ask for auth from PVC owner.");
                return;
            }else if (Context.Message.Channel.Id != 1046883404123222096){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou must use PVC commands in <#1046883404123222096>.");
                return;
            }else if (user == null){
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou didn't specify a user; Identify them using their ``Discord ID`` or ``@Mention``.");
                return;
            }else if (rUser.VoiceChannel == null || rUser.VoiceChannel.CategoryId != 1045032443830353931){
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou are not in a PVC; Join one to use this.");
                return;
            }else{
                await rUser.VoiceChannel.RemovePermissionOverwriteAsync(user);
            }
        }
    }
}
*/