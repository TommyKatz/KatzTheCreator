/*using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.PVCModules{
    public class iBan : ModuleBase<SocketCommandContext>{

        [Command("iban")]
        public async Task BanVCUser(SocketGuildUser user = null){

            // view channel = PVC Owner
            // speak = PVC Mod
            // connect = auth override

            var rUser = Context.User as SocketGuildUser;

            if (rUser.VoiceChannel == null) return;

            var permHasValue = rUser.VoiceChannel.GetPermissionOverwrite(rUser).HasValue;

            // does the user using command have any overwrites?
            if (!permHasValue){
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou must have permission to use this; Create your own PVC or ask for auth from PVC owner.");
                return;
            } else {
                var permCheckOwner = rUser.VoiceChannel.GetPermissionOverwrite(rUser).Value.ViewChannel;
                var permCheckMod = rUser.VoiceChannel.GetPermissionOverwrite(rUser).Value.Speak;
                var userPermHasValue = rUser.VoiceChannel.GetPermissionOverwrite(user).HasValue;

                if (permCheckOwner == 0){
                    if (Context.Message.Channel.Id != 1046883404123222096){
                        await Context.Message.DeleteAsync();
                        await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                        "***Uh oh! Something went wrong...***\n\nYou must use PVC commands in <#1046883404123222096>.");
                        return;
                    }else if (user == null){
                        await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                        "***Uh oh! Something went wrong...***\n\nYou didn't specify a user; Identify them using their ``Discord ID`` or ``@Mention``.");
                        return;
                    }else if (rUser.VoiceChannel.CategoryId != 1045032443830353931){
                        await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                        "***Uh oh! Something went wrong...***\n\nYou are not in a PVC; Join one to use this.");
                        return;
                    }else if (userPermHasValue){
                        var userPermConnect = rUser.VoiceChannel.GetPermissionOverwrite(user).Value.Connect;

                        if (userPermConnect == PermValue.Deny){
                            await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                            "***Uh oh! Something went wrong...***\n\nThis member is already banned in this PVC.");
                            return;
                        }else{
                            var newOverwrites = new OverwritePermissions(connect: PermValue.Deny);
                            await rUser.VoiceChannel.AddPermissionOverwriteAsync(user, newOverwrites);
                            if (user.VoiceChannel == rUser.VoiceChannel){

                                await user.ModifyAsync(x => { x.Channel = null; });
                            }
                        }

                    }else{
                        var newOverwrites = new OverwritePermissions(connect: PermValue.Deny);
                        await rUser.VoiceChannel.AddPermissionOverwriteAsync(user, newOverwrites);
                        if (user.VoiceChannel == rUser.VoiceChannel){

                            await user.ModifyAsync(x => { x.Channel = null; });
                        }
                    }
                }

                if (permCheckMod == 0){
                    if (userPermHasValue){
                        var userPermOwner = rUser.VoiceChannel.GetPermissionOverwrite(user).Value.ViewChannel;
                        var userPermMod = rUser.VoiceChannel.GetPermissionOverwrite(user).Value.Speak;
                        var userPermConnect = rUser.VoiceChannel.GetPermissionOverwrite(user).Value.Connect;

                        if (userPermOwner == 0 || userPermMod == 0){
                            await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                            "***Uh oh! Something went wrong...***\n\nYou don't have permission to use this; This member has elevated permissions in this PVC.");
                            return;
                        }

                        if (userPermConnect == PermValue.Deny){
                            await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                            "***Uh oh! Something went wrong...***\n\nThis member is already banned in this PVC.");
                            return;
                        }

                        if (permCheckOwner != 0 || permCheckMod != 0){
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
                        }else if (rUser.VoiceChannel.CategoryId != 1045032443830353931){
                            await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                            "***Uh oh! Something went wrong...***\n\nYou are not in a PVC; Join one to use this.");
                            return;
                        }else{
                            var newOverwrites = new OverwritePermissions(connect: PermValue.Deny);
                            await rUser.VoiceChannel.AddPermissionOverwriteAsync(user, newOverwrites);
                            if (user.VoiceChannel == rUser.VoiceChannel){

                                await user.ModifyAsync(x => { x.Channel = null; });
                            }

                        }

                    }
                }
            }
        }
    }
}    
*/
