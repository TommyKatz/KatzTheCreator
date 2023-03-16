/*using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.PVCModules
{
    public class iUnlock : ModuleBase<SocketCommandContext> {

        [Command("iunlock")]
        public async Task UnlockPVC(){

            var rUser = Context.User as SocketGuildUser;
            var permHasValue = rUser.VoiceChannel.GetPermissionOverwrite(rUser).HasValue;

            if (!permHasValue){
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou must have permission to use this; Create your own PVC or ask for auth from PVC owner.");
                return;
            }

            var permCheck = rUser.VoiceChannel.GetPermissionOverwrite(rUser).Value.Connect;

            if (permCheck != 0){
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou must have permission to use this; Create your own PVC or ask for auth from PVC owner.");
                return;
            }else if (Context.Message.Channel.Id != 1046883404123222096){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou must use PVC commands in <#1046883404123222096>.");
                return;
            }else if (rUser.VoiceChannel == null || rUser.VoiceChannel.CategoryId != 1045032443830353931){
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou are not in a PVC; Join one to use this.");
                return;
            }else{
                var everyoneRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 960957925143224340);
                var newOverwrites = new OverwritePermissions(connect: PermValue.Inherit);
                await rUser.VoiceChannel.AddPermissionOverwriteAsync(everyoneRole, newOverwrites);
            }

        }

    }
}
*/