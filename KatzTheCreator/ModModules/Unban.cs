using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.ModModules{
    public class Unban : ModuleBase<SocketCommandContext>{
        [Command("unban")]
        public async Task UnbanUser(ulong userToBeUnbanned = default, [Remainder] string unbanReason = null){

            var rUser = Context.User as SocketGuildUser;
            var rUserHighestRole = rUser.Roles.OrderBy(r => r.Position).Last();
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var serverName = Context.Guild.Name;
            var serverIconUrl = Context.Guild.IconUrl;
            var loggingChannel = Context.Guild.GetChannel(965699174358216744) as SocketTextChannel; 

            if (rUser.Roles.Contains(directorRole)){

                if (userToBeUnbanned == default){
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nYou didn't specify a user; Identify them using their ``Discord ID`` or ``@Mention``.");
                    return;
                } else if (string.IsNullOrWhiteSpace(unbanReason)){
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nYou didn't state a reason; A reason must be provided to use this.");
                    return;
                } else {
                    // Sends ban message in current text channel
                    await Context.Guild.RemoveBanAsync(userToBeUnbanned);
                    await Context.Message.DeleteAsync();
                    var builder = new EmbedBuilder()
                    .WithColor(Color.DarkPurple)
                    .WithThumbnailUrl(serverIconUrl)
                    .WithCurrentTimestamp()
                    .WithDescription($"<@{userToBeUnbanned}> **has been unbanned from\n {serverName}.**\n\n **Reason:** {unbanReason}.")
                    .WithFooter(footer =>{
                        footer
                        .WithText($"Unbanned by {rUserHighestRole} | {rUser}")
                        .WithIconUrl(rUser.GetAvatarUrl());
                    });
                    Embed embed = builder.Build();
                    await ReplyAsync(embed: embed);


                    // Sends Embed to Logging Channel
                    var builderTwo = new EmbedBuilder()
                    .WithAuthor($"{rUser} (ID: {rUser.Id})", rUser.GetAvatarUrl())
                    .WithColor(Color.DarkGreen)
                    .WithDescription($"**Unbanned:** <@{userToBeUnbanned}> *(ID: {userToBeUnbanned})*\n**Reason:** {unbanReason}")
                    .WithCurrentTimestamp();
                    Embed embedTwo = builderTwo.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);
                }

            } else{
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou do not have permission to use this.");
            }
        }
    }
}
