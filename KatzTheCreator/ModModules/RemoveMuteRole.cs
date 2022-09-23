using Discord.Commands;
using Discord.WebSocket;
using Discord;

namespace KatzTheCreator.ModModules{
    public class UnmuteRole : ModuleBase<SocketCommandContext>{

        [Command("unmute")]
        public async Task RemoveMuteRoleOfUser(SocketGuildUser userToBeUnmuted = null, [Remainder] string unmuteReason = null){

            var rUser = Context.User as SocketGuildUser;
            var serverName = Context.Guild.Name;
            var serverIconUrl = Context.Guild.IconUrl;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var modRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965726434624688188);
            var mutedRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 966087394040369182);
            var loggingChannel = Context.Guild.GetChannel(965699174358216744) as SocketTextChannel;
            var waitTimeSeven = 7000;

            if (rUser.Roles.Contains(directorRole) || rUser.Roles.Contains(modRole)){

                if (userToBeUnmuted == null){
                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithDescription($"{rUser.Mention}, you didn't specify a user; Identify them using their ``Discord ID`` or ``@Mention``.");
                    Embed embed = embedBuilder.Build();
                    var botReplyFailUser = await ReplyAsync(embed: embed);
                    await Context.Message.DeleteAsync();
                    await Task.Delay(waitTimeSeven);
                    await botReplyFailUser.DeleteAsync();
                    return;
                }

                var userHierachyPos = userToBeUnmuted.Hierarchy;
                var moderatorHierachyPos = rUser.Hierarchy;

                if (string.IsNullOrWhiteSpace(unmuteReason)){
                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{rUser.Mention}, you didn't state a reason; A reason must be provided to use this.");
                        Embed embed = embedBuilder.Build();
                        var botReplyFailReason = await ReplyAsync(embed: embed);
                        await Context.Message.DeleteAsync();
                        await Task.Delay(waitTimeSeven);
                        await botReplyFailReason.DeleteAsync();
                        return;
                } else if (!userToBeUnmuted.Roles.Contains(mutedRole)){
                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{rUser.Mention}, user **{userToBeUnmuted.Username}** is not muted.");
                        Embed embed = embedBuilder.Build();
                        var botReplyFailReason = await ReplyAsync(embed: embed);
                        await Context.Message.DeleteAsync();
                        await Task.Delay(waitTimeSeven);
                        await botReplyFailReason.DeleteAsync();
                        return;
                } else {
                    await Context.Message.DeleteAsync();

                        try{
                        await userToBeUnmuted.SendMessageAsync($"You have been unmuted in **Bugs By Daylight** for **{unmuteReason}**.\n~\n Withdrawn by Staff Member: {rUser.Mention}\n~\n*You may now type and rejoin voice channels again.*");

                        } catch {
                        var amountBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{rUser.Mention}, this user's DMs are disabled; A message could not be sent to the muted user.");
                        Embed tryEmbed = amountBuilder.Build();
                        var botReplyFailPerms = await ReplyAsync(embed: tryEmbed);
                        }

                    await userToBeUnmuted.RemoveRoleAsync(mutedRole);

                    var builder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithThumbnailUrl(serverIconUrl)
                        .WithCurrentTimestamp()
                        .WithDescription($"{userToBeUnmuted.Mention} **has been unmuted in\n {serverName}.**\n\n **Reason:** {unmuteReason}.")
                        .WithFooter(footer => {
                            footer
                            .WithText($"Unmuted by Server Staff | {rUser}")
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
                var amountBuilder = new EmbedBuilder()
                    .WithColor(Color.DarkPurple)
                    .WithDescription($"{rUser.Mention}, You do not have permission to use this.");
                Embed embed = amountBuilder.Build();
                var botReplyFailPerms = await ReplyAsync(embed: embed);
                await Context.Message.DeleteAsync();
                await Task.Delay(waitTimeSeven);
                await botReplyFailPerms.DeleteAsync();
            }
        }
    }
}
