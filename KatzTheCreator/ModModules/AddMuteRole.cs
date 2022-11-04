using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;

namespace KatzTheCreator.ModModules{
    public class MuteRole : ModuleBase<SocketCommandContext>{

        [Command("mute")]
        public async Task AddMuteRoleToUser(SocketGuildUser userToBeMuted = null, [Remainder] string muteReason = null){
            var rUser = Context.User as SocketGuildUser;
            var rUserHighestRole = rUser.Roles.OrderBy(r => r.Position).Last();
            var serverName = Context.Guild.Name;
            var serverIconUrl = Context.Guild.IconUrl;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var modRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965726434624688188);
            var repRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 1009266690204385322);
            var mutedRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 966087394040369182);
            var loggingChannel = Context.Guild.GetChannel(965699174358216744) as SocketTextChannel;

            if (rUser.Roles.Contains(directorRole) || rUser.Roles.Contains(modRole)){

                if (userToBeMuted == null || userToBeMuted.IsBot){
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nYou didn't specify a user; Identify them using their ``Discord ID`` or ``@Mention``.");
                    return;
                }

                var userHierachyPos = userToBeMuted.Hierarchy;
                var moderatorHierachyPos = rUser.Hierarchy;

                if (moderatorHierachyPos > userHierachyPos){

                    if (string.IsNullOrWhiteSpace(muteReason)){
                        await Context.Message.DeleteAsync();
                        await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                        "***Uh oh! Something went wrong...***\n\nYou didn't state a reason; A reason must be provided to use this.");
                        return;
                    }
                    else if (userToBeMuted.Roles.Contains(mutedRole)){
                        await Context.Message.DeleteAsync();
                        await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                        "***Uh oh! Something went wrong...***\n\nUser **{userToBeMuted.Username}** has already been muted.");
                        return;
                    } else {
                        await Context.Message.DeleteAsync();

                        if (userToBeMuted.Roles.Contains(repRole)){
                            await userToBeMuted.RemoveRoleAsync(repRole);
                        }

                        try{
                            await userToBeMuted.SendMessageAsync($"You have been muted in **Bugs By Daylight** for **{muteReason}**.\n~\n Issued by {rUserHighestRole}: {rUser.Mention}\n~\n*Please note: All mutes from this server are indefinite.*\n*Any mute reversals are at the discretion of the issuer or the server directors.*");
                        }catch{
                            await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                            "***Uh oh! DM couldn't be sent but action was still was taken...***\n\nThis user's DMs are disabled; A message could not be sent to the muted user.");
                        }

                        await userToBeMuted.AddRoleAsync(mutedRole);
                        await userToBeMuted.ModifyAsync(x => { x.Channel = null; });

                        var builder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithThumbnailUrl(serverIconUrl)
                            .WithCurrentTimestamp()
                            .WithDescription($"{userToBeMuted.Mention} **has been muted in\n {serverName}.**\n\n **Reason:** {muteReason}.")
                            .WithFooter(footer =>{
                                footer
                                .WithText($"Muted by {rUserHighestRole} | {rUser}")
                                .WithIconUrl(rUser.GetAvatarUrl());
                            });
                        Embed embed = builder.Build();
                        await ReplyAsync(embed: embed);

                        // Sends Embed to Logging Channel
                        var builderTwo = new EmbedBuilder()
                        .WithColor(Color.DarkerGrey)
                        .WithThumbnailUrl(userToBeMuted.GetAvatarUrl())
                        .WithAuthor($"{rUser} (ID: {rUser.Id})", rUser.GetAvatarUrl())
                        .WithDescription($"**Muted:** {userToBeMuted} *(ID: {userToBeMuted.Id})*\n**Reason:** {muteReason}")
                        .WithCurrentTimestamp();
                        Embed embedTwo = builderTwo.Build();
                        await loggingChannel.SendMessageAsync(embed: embedTwo);
                    }

                } else {
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nNice try, this user's hierarchy position is higher than yours.");
                }      

            } else {
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou do not have permission to use this.");
            }
        }
    }
}
