﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RequireUserPermissionAttribute = Discord.Commands.RequireUserPermissionAttribute;

namespace KatzTheCreator.ModModules{
    public class Revoke : ModuleBase<SocketCommandContext>{
        [Command("revoke")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RevokeUserAccess(SocketGuildUser userToBeRevoked = null, [Remainder] string revokeReason = null){
            var rUser = Context.User as SocketGuildUser;
            var serverName = Context.Guild.Name;

            if (userToBeRevoked == null){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou didn't specify a user; Identify them using their ``Discord ID`` or ``@Mention``.");
                return; ;
            }

            var userHierachyPos = userToBeRevoked.Hierarchy;
            var moderatorHierachyPos = rUser.Hierarchy;
            var rUserHighestRole = rUser.Roles.MaxBy(r => r.Position);
            var removedDefaults = rUser.Roles.Where(r => r.Color != Color.Default);

            if (moderatorHierachyPos > userHierachyPos){

                if (string.IsNullOrWhiteSpace(revokeReason)){
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nYou didn't state a reason; A reason must be provided to use this.");
                    return;
                }else{
                    await Context.Message.DeleteAsync();

                    try{
                        await userToBeRevoked.SendMessageAsync($"You've been revoked access from **{serverName}** for **{revokeReason}**.\n~\n Issued by {rUserHighestRole}: {rUser.Mention}\n~\n *You must be re-invited to join this server again.*");
                    }catch (Exception){
                        await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                        $"***Uh oh! DM couldn't be sent but action was still was taken...***\n\nThis user's DMs are disabled; A message could not be sent to the revoked user.");
                    }

                    var serverIconUrl = Context.Guild.IconUrl;

                    await userToBeRevoked.KickAsync(revokeReason);

                    if (removedDefaults.Count() != 0){
                        var rUserColor = removedDefaults.MaxBy(r => r.Position).Color;
                        var builder = new EmbedBuilder()
                            .WithColor(rUserColor)
                            .WithThumbnailUrl(serverIconUrl)
                            .WithCurrentTimestamp()
                            .WithDescription($"{userToBeRevoked.Mention} **has been revoked acesss from\n {serverName}.**\n\n **Reason:** {revokeReason}.")
                            .WithFooter(footer => {
                                footer
                                .WithText($"Access Revoked by {rUserHighestRole} | {rUser.Username}")
                                .WithIconUrl(rUser.GetAvatarUrl());
                            });
                        Embed embed = builder.Build();
                        await ReplyAsync(embed: embed);
                    }
                    else{
                        var builder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithThumbnailUrl(serverIconUrl)
                            .WithCurrentTimestamp()
                            .WithDescription($"{userToBeRevoked.Mention} **has been revoked acesss from\n {serverName}.**\n\n **Reason:** {revokeReason}.")
                            .WithFooter(footer => {
                                footer
                                .WithText($"Access Revoked by {rUserHighestRole} | {rUser.Username}")
                                .WithIconUrl(rUser.GetAvatarUrl());
                            });
                        Embed embed = builder.Build();
                        await ReplyAsync(embed: embed);
                    }

                    // Sends Embed to Logging Channel
                    var builderTwo = new EmbedBuilder()
                    .WithColor(Color.Gold)
                    .WithThumbnailUrl(userToBeRevoked.GetAvatarUrl())
                    .WithAuthor($"{rUser.Username} (ID: {rUser.Id})", rUser.GetAvatarUrl())
                    .WithDescription($"**Revoked:** {userToBeRevoked.Mention} *(ID: {userToBeRevoked.Id})*\n**Reason:** {revokeReason}")
                    .WithCurrentTimestamp();
                    Embed embedTwo = builderTwo.Build();

                    var loggingChannel = Context.Guild.GetChannel(965699174358216744) as SocketTextChannel;
                    await loggingChannel.SendMessageAsync(embed: embedTwo);
                }

            }else{
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nNice try, this user's hierarchy position is higher than yours.");
            }
        }
    }
}
