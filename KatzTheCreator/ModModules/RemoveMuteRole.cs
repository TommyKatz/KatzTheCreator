using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RequireUserPermissionAttribute = Discord.Commands.RequireUserPermissionAttribute;

namespace KatzTheCreator.ModModules{
    public class UnmuteRole : ModuleBase<SocketCommandContext>{
        [Command("unmute")]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        public async Task RemoveMuteRoleOfUser(SocketGuildUser userToBeUnmuted = null, [Remainder] string unmuteReason = null){
            var rUser = Context.User as SocketGuildUser;
            var rUserHighestRole = rUser.Roles.MaxBy(r => r.Position);
            var removedDefaults = rUser.Roles.Where(r => r.Color != Color.Default);
            var mutedRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 966087394040369182);
            var serverName = Context.Guild.Name;

            if (userToBeUnmuted == null){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou didn't specify a user; Identify them using their ``Discord ID`` or ``@Mention``.");
                return;
            }

            var userHierachyPos = userToBeUnmuted.Hierarchy;
            var moderatorHierachyPos = rUser.Hierarchy;

            if (string.IsNullOrWhiteSpace(unmuteReason)){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou didn't state a reason; A reason must be provided to use this.");
                return;
            }else if (!userToBeUnmuted.Roles.Contains(mutedRole)){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                $"***Uh oh! Something went wrong...***\n\nUser **{userToBeUnmuted.Username}** is not muted.");
                return;
            }else{
                await Context.Message.DeleteAsync();

                try{
                    await userToBeUnmuted.SendMessageAsync($"You have been unmuted in **{serverName}** for **{unmuteReason}**.\n~\n Withdrawn by {rUserHighestRole}: {rUser.Mention}\n~\n*You may now type and rejoin voice channels again.*");
                }catch{
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    $"***Uh oh! DM couldn't be sent but action was still was taken...***\n\nThis user's DMs are disabled; A message could not be sent to the muted user.");
                }

                await userToBeUnmuted.RemoveRoleAsync(mutedRole);
                var serverIconUrl = Context.Guild.IconUrl;

                if (removedDefaults.Count() != 0){
                    var rUserColor = removedDefaults.MaxBy(r => r.Position).Color;
                    var builder = new EmbedBuilder()
                        .WithColor(rUserColor)
                        .WithThumbnailUrl(serverIconUrl)
                        .WithCurrentTimestamp()
                        .WithDescription($"{userToBeUnmuted.Mention} **has been unmuted in\n {serverName}.**\n\n **Reason:** {unmuteReason}.")
                        .WithFooter(footer => {
                            footer
                            .WithText($"Unmuted by {rUserHighestRole} | {rUser.Username}")
                            .WithIconUrl(rUser.GetAvatarUrl());
                        });
                    Embed embed = builder.Build();
                    await ReplyAsync(embed: embed);
                }else{
                    var builder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithThumbnailUrl(serverIconUrl)
                        .WithCurrentTimestamp()
                        .WithDescription($"{userToBeUnmuted.Mention} **has been unmuted in\n {serverName}.**\n\n **Reason:** {unmuteReason}.")
                        .WithFooter(footer => {
                            footer
                            .WithText($"Unmuted by {rUserHighestRole} | {rUser.Username}")
                            .WithIconUrl(rUser.GetAvatarUrl());
                        });
                    Embed embed = builder.Build();
                    await ReplyAsync(embed: embed);
                }
                    
                // Sends Embed to Logging Channel
                var loggingChannel = Context.Guild.GetChannel(965699174358216744) as SocketTextChannel;
                var builderTwo = new EmbedBuilder()
                    .WithColor(Color.LighterGrey)
                    .WithThumbnailUrl(userToBeUnmuted.GetAvatarUrl())
                    .WithAuthor($"{rUser.Username} (ID: {rUser.Id})", rUser.GetAvatarUrl())
                    .WithDescription($"**Unmuted:** {userToBeUnmuted.Username} *(ID: {userToBeUnmuted.Id})*\n**Reason:** {unmuteReason}")
                    .WithCurrentTimestamp();
                Embed embedTwo = builderTwo.Build();
                await loggingChannel.SendMessageAsync(embed: embedTwo);
            }
        }
    }
}
