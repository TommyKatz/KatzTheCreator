using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.ModModules{
    public class MuteRole : ModuleBase<SocketCommandContext>{

        [Command("mute")]
        public async Task AddMuteRoleToUser(SocketGuildUser userToBeMuted = null, [Remainder] string muteReason = null){
            var rUser = Context.User as SocketGuildUser;
            var serverName = Context.Guild.Name;
            var serverIconUrl = Context.Guild.IconUrl;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var modRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965726434624688188);
            var mutedRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 966087394040369182);
            var loggingChannel = Context.Guild.GetChannel(965699174358216744) as SocketTextChannel;
            var waitTimeSeven = 7000;

            if (rUser.Roles.Contains(directorRole) || rUser.Roles.Contains(modRole)){

                if (userToBeMuted == null){
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

                var userHierachyPos = userToBeMuted.Hierarchy;
                var moderatorHierachyPos = rUser.Hierarchy;

                if (moderatorHierachyPos > userHierachyPos){

                    if (string.IsNullOrWhiteSpace(muteReason)){
                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{rUser.Mention}, you didn't state a reason; A reason must be provided to use this.");
                        Embed embed = embedBuilder.Build();
                        var botReplyFailReason = await ReplyAsync(embed: embed);
                        await Context.Message.DeleteAsync();
                        await Task.Delay(waitTimeSeven);
                        await botReplyFailReason.DeleteAsync();
                        return;
                    } else if (userToBeMuted.Roles.Contains(mutedRole)){
                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{rUser.Mention}, user **{userToBeMuted.Username}** has already been muted.");
                        Embed embed = embedBuilder.Build();
                        var botReplyFailReason = await ReplyAsync(embed: embed);
                        await Context.Message.DeleteAsync();
                        await Task.Delay(waitTimeSeven);
                        await botReplyFailReason.DeleteAsync();
                        return;
                    } else {
                        await Context.Message.DeleteAsync();
                            try{
                            await userToBeMuted.SendMessageAsync($"You have been muted in **Bugs By Daylight** for **{muteReason}**.\n~\n Issued by Staff Member: {rUser.Mention}\n~\n*Please note: All mutes from this server are indefinite.*\n*Any mute reversals are at the discretion of the issuer or the server directors.*");
                            } catch{
                            var amountBuilder = new EmbedBuilder()
                                .WithColor(Color.DarkPurple)
                                .WithDescription($"{rUser.Mention}, this user's DMs are disabled; A message could not be sent to the muted user.");
                            Embed tryEmbed = amountBuilder.Build();
                            var botReplyFailPerms = await ReplyAsync(embed: tryEmbed);
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
                                .WithText($"Muted by Server Staff | {rUser}")
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
                    var amountBuilder = new EmbedBuilder()
                    .WithColor(Color.DarkPurple)
                    .WithDescription($"{rUser.Mention}, nice try, this user's hierarchy position is higher than yours.");
                    Embed embed = amountBuilder.Build();
                    var botReplyFailPerms = await ReplyAsync(embed: embed);
                    await Context.Message.DeleteAsync();
                    await Task.Delay(waitTimeSeven);
                    await botReplyFailPerms.DeleteAsync();
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
