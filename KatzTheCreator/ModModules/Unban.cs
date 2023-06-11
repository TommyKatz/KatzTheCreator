using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RequireUserPermissionAttribute = Discord.Commands.RequireUserPermissionAttribute;

namespace KatzTheCreator.ModModules{
    public class Unban : ModuleBase<SocketCommandContext>{
        [Command("unban")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task UnbanUser(ulong userToBeUnbanned = default, [Remainder] string unbanReason = null){
            var rUser = Context.User as SocketGuildUser;

            if (userToBeUnbanned == default){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou didn't specify a user; Identify them using their ``Discord ID`` or ``@Mention``.");
                return;
            }else if (string.IsNullOrWhiteSpace(unbanReason)){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou didn't state a reason; A reason must be provided to use this.");
                return;
            }else{
                var rUserHighestRole = rUser.Roles.MaxBy(r => r.Position);
                var removedDefaults = rUser.Roles.Where(r => r.Color != Color.Default);
                var serverName = Context.Guild.Name;
                var serverIconUrl = Context.Guild.IconUrl;

                await Context.Guild.RemoveBanAsync(userToBeUnbanned);
                await Context.Message.DeleteAsync();

                if (removedDefaults.Count() != 0){
                    var rUserColor = removedDefaults.MaxBy(r => r.Position).Color;
                    var builder = new EmbedBuilder()
                        .WithColor(rUserColor)
                        .WithThumbnailUrl(serverIconUrl)
                        .WithCurrentTimestamp()
                        .WithDescription($"<@{userToBeUnbanned}> **has been unbanned from\n {serverName}.**\n\n **Reason:** {unbanReason}.")
                        .WithFooter(footer =>
                        {
                            footer
                            .WithText($"Unbanned by {rUserHighestRole} | {rUser.Username}")
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
                        .WithDescription($"<@{userToBeUnbanned}> **has been unbanned from\n {serverName}.**\n\n **Reason:** {unbanReason}.")
                        .WithFooter(footer =>
                        {
                            footer
                            .WithText($"Unbanned by {rUserHighestRole} | {rUser}")
                            .WithIconUrl(rUser.GetAvatarUrl());
                        });
                    Embed embed = builder.Build();
                    await ReplyAsync(embed: embed);
                }

                // Sends Embed to Logging Channel
                var builderTwo = new EmbedBuilder()
                .WithAuthor($"{rUser.Username} (ID: {rUser.Id})", rUser.GetAvatarUrl())
                .WithColor(Color.DarkGreen)
                .WithDescription($"**Unbanned:** <@{userToBeUnbanned}> *(ID: {userToBeUnbanned})*\n**Reason:** {unbanReason}")
                .WithCurrentTimestamp();
                Embed embedTwo = builderTwo.Build();

                var loggingChannel = Context.Guild.GetChannel(965699174358216744) as SocketTextChannel;
                await loggingChannel.SendMessageAsync(embed: embedTwo);
            }
        }
    }
}
