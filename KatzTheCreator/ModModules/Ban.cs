using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.ModModules{
    public class Ban : ModuleBase<SocketCommandContext>{
        [Command("ban")]
        public async Task BanUserMention(ulong userToBeBanned = default, [Remainder] string banReason = null){
            var rUser = Context.User as SocketGuildUser;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var serverName = Context.Guild.Name;
            var serverIconUrl = Context.Guild.IconUrl;
            var loggingChannel = Context.Guild.GetChannel(965699174358216744) as SocketTextChannel;
            var waitTimeSeven = 7000;

            if (rUser.Roles.Contains(directorRole)){

                if (userToBeBanned == default){
                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithDescription($"{rUser.Mention}, you didn't specify a user; Identify them using their ``Discord ID``.");
                    Embed embed = embedBuilder.Build();
                    var botReplyFailUser = await ReplyAsync(embed: embed);
                    await Context.Message.DeleteAsync();
                    await Task.Delay(waitTimeSeven);
                    await botReplyFailUser.DeleteAsync();
                    return;
                }

                var moderatorHierarchyPos = rUser.Hierarchy;

                if (Context.Guild.GetUser(userToBeBanned) == null){

                    if (string.IsNullOrWhiteSpace(banReason)){
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
                        await Context.Guild.AddBanAsync(userToBeBanned, 1, $"{rUser}: {banReason}");
                        await Context.Message.DeleteAsync();
                        var builder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithThumbnailUrl(serverIconUrl)
                        .WithCurrentTimestamp()
                        .WithDescription($"<@{userToBeBanned}> **has been banned from\n {serverName}.**\n\n **Reason:** {banReason}.")
                        .WithFooter(footer =>{
                            footer
                            .WithText($"Banned by Staff Member | {rUser}")
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

                } else {
                    var userHierachyPos = Context.Guild.GetUser(userToBeBanned).Hierarchy;

                    if (moderatorHierarchyPos > userHierachyPos){

                        if (string.IsNullOrWhiteSpace(banReason)){
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
                                await Context.Guild.GetUser(userToBeBanned).SendMessageAsync($"You been banned from **Bugs By Daylight** for **{banReason}**.\n~\n Issued by Staff Member: {rUser.Mention}\n~\n *Please note: All bans from this server are permanent and cannot be appealed.\n Any ban reversals are at the discretion of the Server Directors.*");

                            } catch (Exception messageNotDelivered){
                                var amountBuilder = new EmbedBuilder()
                                    .WithColor(Color.DarkPurple)
                                    .WithDescription($"{rUser.Mention}, this user's DMs are disabled; A message could not be sent to the banned user.");
                                Embed tryEmbed = amountBuilder.Build();
                                var botReplyFailPerms = await ReplyAsync(embed: tryEmbed);
                            }

                            await Context.Guild.AddBanAsync(userToBeBanned, 1, $"{rUser}: {banReason}");
                            var builder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithThumbnailUrl(serverIconUrl)
                            .WithCurrentTimestamp()
                            .WithDescription($"<@{userToBeBanned}> **has been banned from\n {serverName}.**\n\n **Reason:** {banReason}.")
                            .WithFooter(footer =>{
                                footer
                                .WithText($"Banned by Staff Member | {rUser}")
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
