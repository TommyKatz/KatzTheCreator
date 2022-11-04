using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Channels;
using System;

namespace KatzTheCreator.ModModules{
    public class Bring : ModuleBase<SocketCommandContext>{

        [Command("bring")]
        [Alias("get")]
        public async Task BringUser(SocketGuildUser userToBeBrought = null){

            var rUser = Context.User as SocketGuildUser;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var modRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965726434624688188);
            var waitTimeSeven = 7000;


            if (rUser.Roles.Contains(directorRole) || rUser.Roles.Contains(modRole)){

                if (userToBeBrought == null){
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nYou didn't specify a user; Identify them using their ``Discord ID`` or ``@Mention``.");
                    return;
                } else if (userToBeBrought.VoiceChannel == null){

                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nThe user you are trying to get is not in a voice channel.");
                    return;
                } else if (rUser.VoiceChannel == null){

                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nYou are not in a voice channel; Join one to use this.");
                    return;
                } else if (rUser.VoiceChannel == userToBeBrought.VoiceChannel){

                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nThis user's already in your voice channel.");
                    return;
                } else {
                    var rUserVC = rUser.VoiceChannel.Id;
                    await userToBeBrought.ModifyAsync(x => { x.ChannelId = rUserVC; });
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
