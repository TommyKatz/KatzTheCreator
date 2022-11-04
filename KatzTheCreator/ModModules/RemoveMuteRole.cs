using Discord.Commands;
using Discord.WebSocket;
using Discord;

namespace KatzTheCreator.ModModules{
    public class UnmuteRole : ModuleBase<SocketCommandContext>{

        [Command("unmute")]
        public async Task RemoveMuteRoleOfUser(SocketGuildUser userToBeUnmuted = null, [Remainder] string unmuteReason = null){
            var rUser = Context.User as SocketGuildUser;
            var rUserHighestRole = rUser.Roles.OrderBy(r => r.Position).Last();
            var serverName = Context.Guild.Name;
            var serverIconUrl = Context.Guild.IconUrl;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var modRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965726434624688188);
            var mutedRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 966087394040369182);
            var loggingChannel = Context.Guild.GetChannel(965699174358216744) as SocketTextChannel;
            var waitTimeSeven = 7000;

            if (rUser.Roles.Contains(directorRole) || rUser.Roles.Contains(modRole)){

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
                } else if (!userToBeUnmuted.Roles.Contains(mutedRole)){
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    $"***Uh oh! Something went wrong...***\n\nUser **{userToBeUnmuted.Username}** is not muted.");
                    return;
                } else {
                    await Context.Message.DeleteAsync();

                        try{
                            await userToBeUnmuted.SendMessageAsync($"You have been unmuted in **Bugs By Daylight** for **{unmuteReason}**.\n~\n Withdrawn by {rUserHighestRole}: {rUser.Mention}\n~\n*You may now type and rejoin voice channels again.*");
                        } catch {
                            await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                            $"***Uh oh! DM couldn't be sent but action was still was taken...***\n\nThis user's DMs are disabled; A message could not be sent to the muted user.");
                        }

                    await userToBeUnmuted.RemoveRoleAsync(mutedRole);

                    var builder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithThumbnailUrl(serverIconUrl)
                        .WithCurrentTimestamp()
                        .WithDescription($"{userToBeUnmuted.Mention} **has been unmuted in\n {serverName}.**\n\n **Reason:** {unmuteReason}.")
                        .WithFooter(footer => {
                            footer
                            .WithText($"Unmuted by {rUserHighestRole} | {rUser}")
                            .WithIconUrl(rUser.GetAvatarUrl());
                        });
                    Embed embed = builder.Build();
                    await ReplyAsync(embed: embed);

                        // Sends Embed to Logging Channel
                    var builderTwo = new EmbedBuilder()
                        .WithColor(Color.LighterGrey)
                        .WithThumbnailUrl(userToBeUnmuted.GetAvatarUrl())
                        .WithAuthor($"{rUser} (ID: {rUser.Id})", rUser.GetAvatarUrl())
                        .WithDescription($"**Unmuted:** {userToBeUnmuted} *(ID: {userToBeUnmuted.Id})*\n**Reason:** {unmuteReason}")
                        .WithCurrentTimestamp();
                    Embed embedTwo = builderTwo.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);
                }

            } else {
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou do not have permission to use this.");
            }
        }
    }
}
