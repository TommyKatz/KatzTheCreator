using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RequireUserPermissionAttribute = Discord.Commands.RequireUserPermissionAttribute;

namespace KatzTheCreator.ModModules{
    public class MuteRole : ModuleBase<SocketCommandContext>{

        [Command("mute")]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        public async Task AddMuteRoleToUser(SocketGuildUser userToBeMuted = null, [Remainder] string muteReason = null){
            var rUser = Context.User as SocketGuildUser;
            var serverName = Context.Guild.Name;

            if (userToBeMuted == null || userToBeMuted.IsBot){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou didn't specify a user; Identify them using their ``Discord ID`` or ``@Mention``.");
                return;
            }

            var userHierachyPos = userToBeMuted.Hierarchy;
            var moderatorHierachyPos = rUser.Hierarchy;

            if (moderatorHierachyPos > userHierachyPos){

                var mutedRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 966087394040369182);

                if (string.IsNullOrWhiteSpace(muteReason)){
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nYou didn't state a reason; A reason must be provided to use this.");
                    return;
                }else if (userToBeMuted.Roles.Contains(mutedRole)){
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    $"***Uh oh! Something went wrong...***\n\nUser **{userToBeMuted.Username}** has already been muted.");
                    return;
                }else{
                    await Context.Message.DeleteAsync();

                    var repRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 1009266690204385322);

                    if (userToBeMuted.Roles.Contains(repRole)){
                        await userToBeMuted.RemoveRoleAsync(repRole);
                    }
                    var rUserHighestRole = rUser.Roles.MaxBy(r => r.Position);
                    var removedDefaults = rUser.Roles.Where(r => r.Color != Color.Default);

                    try{
                        await userToBeMuted.SendMessageAsync($"You have been muted in **{serverName}** for **{muteReason}**.\n~\n Issued by {rUserHighestRole}: {rUser.Mention}\n~\n*Please note: All mutes from this server are indefinite.*\n*Any mute reversals are at the discretion of the issuer or the server directors.*");
                    }catch{
                        await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                        "***Uh oh! DM couldn't be sent but action was still was taken...***\n\nThis user's DMs are disabled; A message could not be sent to the muted user.");
                    }

                    await userToBeMuted.AddRoleAsync(mutedRole);
                    await userToBeMuted.ModifyAsync(x => { x.Channel = null; });
                    var serverIconUrl = Context.Guild.IconUrl;
                    

                    if (removedDefaults.Count() != 0){
                        var rUserColor = removedDefaults.MaxBy(r => r.Position).Color;

                        var builder = new EmbedBuilder()
                        .WithColor(rUserColor)
                        .WithThumbnailUrl(serverIconUrl)
                        .WithCurrentTimestamp()
                        .WithDescription($"{userToBeMuted.Mention} **has been muted in\n {serverName}.**\n\n **Reason:** {muteReason}.")
                        .WithFooter(footer => {
                            footer
                            .WithText($"Muted by {rUserHighestRole} | {rUser}")
                            .WithIconUrl(rUser.GetAvatarUrl());
                        });
                        Embed embed = builder.Build();
                        await ReplyAsync(embed: embed);

                        // Sends Embed to Logging Channel
                        var loggingChannel = Context.Guild.GetChannel(965699174358216744) as SocketTextChannel;
                        var builderTwo = new EmbedBuilder()
                        .WithColor(Color.DarkerGrey)
                        .WithThumbnailUrl(userToBeMuted.GetAvatarUrl())
                        .WithAuthor($"{rUser} (ID: {rUser.Id})", rUser.GetAvatarUrl())
                        .WithDescription($"**Muted:** {userToBeMuted} *(ID: {userToBeMuted.Id})*\n**Reason:** {muteReason}")
                        .WithCurrentTimestamp();
                        Embed embedTwo = builderTwo.Build();
                        await loggingChannel.SendMessageAsync(embed: embedTwo);

                    }else{
                        var builder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithThumbnailUrl(serverIconUrl)
                        .WithCurrentTimestamp()
                        .WithDescription($"{userToBeMuted.Mention} **has been muted in\n {serverName}.**\n\n **Reason:** {muteReason}.")
                        .WithFooter(footer => {
                            footer
                            .WithText($"Muted by {rUserHighestRole} | {rUser}")
                            .WithIconUrl(rUser.GetAvatarUrl());
                        });
                        Embed embed = builder.Build();
                        await ReplyAsync(embed: embed);

                        // Sends Embed to Logging Channel
                        var loggingChannel = Context.Guild.GetChannel(965699174358216744) as SocketTextChannel;
                        var builderTwo = new EmbedBuilder()
                        .WithColor(Color.DarkerGrey)
                        .WithThumbnailUrl(userToBeMuted.GetAvatarUrl())
                        .WithAuthor($"{rUser} (ID: {rUser.Id})", rUser.GetAvatarUrl())
                        .WithDescription($"**Muted:** {userToBeMuted} *(ID: {userToBeMuted.Id})*\n**Reason:** {muteReason}")
                        .WithCurrentTimestamp();
                        Embed embedTwo = builderTwo.Build();
                        await loggingChannel.SendMessageAsync(embed: embedTwo);
                    }
                }

            } else {
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nNice try, this user's hierarchy position is higher than yours.");
            }
        }
    }
}
