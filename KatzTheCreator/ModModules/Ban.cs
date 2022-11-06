using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RequireUserPermissionAttribute = Discord.Commands.RequireUserPermissionAttribute;

namespace KatzTheCreator.ModModules{
    public class Ban : ModuleBase<SocketCommandContext>{
        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanUserMention(ulong userToBeBanned = default, [Remainder] string banReason = null){
            var rUser = Context.User as SocketGuildUser;
            var rUserHighestRole = rUser.Roles.OrderBy(r => r.Position).Last();
            var serverName = Context.Guild.Name;
            var serverIconUrl = Context.Guild.IconUrl;
            var loggingChannel = Context.Guild.GetChannel(965699174358216744) as SocketTextChannel;

            if(userToBeBanned == default){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou didn't specify a user; Identify them using their ``Discord ID``.");
                return;
            }

            var moderatorHierarchyPos = rUser.Hierarchy;
            var currentBans = await Context.Guild.GetBansAsync().FlattenAsync();
            var userIsBanned = currentBans.Select(b => b.User).Where(u => u.Id == userToBeBanned).Any();

            if (Context.Guild.GetUser(userToBeBanned) == default){

                if (string.IsNullOrWhiteSpace(banReason)){
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nYou didn't state a reason; A reason must be provided to use this.");
                    return;
                }else if (userIsBanned){
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    $"***Uh oh! Something went wrong...***\n\nUser **<@{userToBeBanned}>** is already banned.");
                    return;
                }else{
                    await Context.Guild.AddBanAsync(userToBeBanned, 1, $"{rUser}: {banReason}");
                    await Context.Message.DeleteAsync();
                    var builder = new EmbedBuilder()
                    .WithColor(Color.DarkPurple)
                    .WithThumbnailUrl(serverIconUrl)
                    .WithCurrentTimestamp()
                    .WithDescription($"<@{userToBeBanned}> **has been banned from\n {serverName}.**\n\n **Reason:** {banReason}.")
                    .WithFooter(footer => {
                        footer
                        .WithText($"Banned by {rUserHighestRole} | {rUser}")
                        .WithIconUrl(rUser.GetAvatarUrl());
                    });
                    Embed embed = builder.Build();
                    await ReplyAsync(embed: embed);

                    // Sends Embed to Logging Channel
                    var builderTwo = new EmbedBuilder()
                    .WithColor(Color.DarkRed)
                    .WithAuthor($"{rUser} (ID: {rUser.Id})", rUser.GetAvatarUrl())
                    .WithDescription($"**Banned:** <@{userToBeBanned}> *(ID: {userToBeBanned})*\n**Reason:** {banReason}")
                    .WithCurrentTimestamp();
                    Embed embedTwo = builderTwo.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);
                }
            }else{
                var userHierachyPos = Context.Guild.GetUser(userToBeBanned).Hierarchy;

                if (moderatorHierarchyPos > userHierachyPos){

                    if (string.IsNullOrWhiteSpace(banReason)){
                        await Context.Message.DeleteAsync();
                        await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                        "***Uh oh! Something went wrong...***\n\nYou didn't state a reason; A reason must be provided to use this.");
                        return;
                    }else{
                        await Context.Message.DeleteAsync();

                        try{
                            await Context.Guild.GetUser(userToBeBanned).SendMessageAsync($"You been banned from **Bugs By Daylight** for **{banReason}**.\n~\n Issued by {rUserHighestRole}: {rUser.Mention}\n~\n *Please note: All bans from this server are permanent and cannot be appealed.\n Any ban reversals are at the discretion of the Server Directors.*");
                        }catch (Exception){
                            await rUser.SendMessageAsync($"***Uh oh! DM couldn't be sent but action was still was taken...***\n\nThis user's DMs are disabled; A message could not be sent to the banned user.");
                        }

                        await Context.Guild.AddBanAsync(userToBeBanned, 1, $"{rUser}: {banReason}");
                        var builder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithThumbnailUrl(serverIconUrl)
                        .WithCurrentTimestamp()
                        .WithDescription($"<@{userToBeBanned}> **has been banned from\n {serverName}.**\n\n **Reason:** {banReason}.")
                        .WithFooter(footer => {
                            footer
                            .WithText($"Banned by {rUserHighestRole} | {rUser}")
                            .WithIconUrl(rUser.GetAvatarUrl());
                        });
                        Embed embed = builder.Build();
                        await ReplyAsync(embed: embed);

                        // Sends Embed to Logging Channel
                        var builderTwo = new EmbedBuilder()
                        .WithColor(Color.DarkRed)
                        .WithThumbnailUrl(Context.Guild.GetUser(userToBeBanned).GetAvatarUrl())
                        .WithAuthor($"{rUser} (ID: {rUser.Id})", rUser.GetAvatarUrl())
                        .WithDescription($"**Banned:** <@{userToBeBanned}> *(ID: {userToBeBanned})*\n**Reason:** {banReason}")
                        .WithCurrentTimestamp();
                        Embed embedTwo = builderTwo.Build();
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
}
