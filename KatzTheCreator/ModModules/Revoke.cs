using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.ModModules{
    public class Revoke : ModuleBase<SocketCommandContext>{
        [Command("revoke")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task RevokeUserAccess(SocketGuildUser userToBeRevoked = null, [Remainder] string revokeReason = null){

            var rUser = Context.User as SocketGuildUser;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var serverName = Context.Guild.Name;
            var serverIconUrl = Context.Guild.IconUrl;
            var loggingChannel = Context.Guild.GetChannel(965699174358216744) as SocketTextChannel;
            var waitTimeSeven = 7000;

            if (rUser.Roles.Contains(directorRole)){

                if (userToBeRevoked == null){
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

                var userHierachyPos = userToBeRevoked.Hierarchy;
                var moderatorHierachyPos = rUser.Hierarchy;

                if (moderatorHierachyPos > userHierachyPos){
                    
                    if (string.IsNullOrWhiteSpace(revokeReason)){
                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{rUser.Mention}, you didn't state a reason; A reason must be provided to use this.");
                        Embed embed = embedBuilder.Build();
                        var botReplyFailReason = await ReplyAsync(embed: embed);
                        await Context.Message.DeleteAsync();
                        await Task.Delay(waitTimeSeven);
                        await botReplyFailReason.DeleteAsync();
                        return;
                    } else {
                        await Context.Message.DeleteAsync();

                            try{
                                await userToBeRevoked.SendMessageAsync($"You been revoked access from **Bugs By Daylight** for **{revokeReason}**.\n~\n Issued by Staff Member: {rUser.Mention}\n~\n *You must be re-invited to join this server again.*");

                            } catch (Exception messageNotDelivered){
                            var amountBuilder = new EmbedBuilder()
                                .WithColor(Color.DarkPurple)
                                .WithDescription($"{rUser.Mention}, this user's DMs are disabled; A message could not be sent to the revoked user.");
                            Embed tryEmbed = amountBuilder.Build();
                            var botReplyFailPerms = await ReplyAsync(embed: tryEmbed);
                            }

                        await userToBeRevoked.KickAsync(revokeReason);
                        var builder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithThumbnailUrl(serverIconUrl)
                        .WithCurrentTimestamp()
                        .WithDescription($"{userToBeRevoked.Mention} **has been revoked acesss from\n {serverName}.**\n\n **Reason:** {revokeReason}.")
                        .WithFooter(footer =>{
                            footer
                            .WithText($"Access Revoked by Staff Member | {rUser}")
                            .WithIconUrl(rUser.GetAvatarUrl());
                        });
                        Embed embed = builder.Build();
                        await ReplyAsync(embed: embed);


                        // Sends Embed to Logging Channel
                        var builderTwo = new EmbedBuilder()
                        .WithColor(Color.Gold)
                        .WithThumbnailUrl(userToBeRevoked.GetAvatarUrl())
                        .WithAuthor($"{rUser} (ID: {rUser.Id})", rUser.GetAvatarUrl())
                        .WithDescription($"**Revoked:** {userToBeRevoked.Mention} *(ID: {userToBeRevoked.Id})*\n**Reason:** {revokeReason}")
                        .WithCurrentTimestamp();
                        Embed embedTwo = builderTwo.Build();
                        await loggingChannel.SendMessageAsync(embed: embedTwo);
                    }

                }  else {
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
                    .WithDescription($"{rUser.Mention}, you do not have permission to use this.");
                Embed embed = amountBuilder.Build();
                var botReplyFailPerms = await ReplyAsync(embed: embed);
                await Context.Message.DeleteAsync();
                await Task.Delay(waitTimeSeven);
                await botReplyFailPerms.DeleteAsync();
            }

            

            
        }
    }
}
