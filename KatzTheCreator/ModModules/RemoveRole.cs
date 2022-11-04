using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.ModModules{
    public class RemoveRole : ModuleBase<SocketCommandContext>{
        [Command("removerole")]
        public async Task SubtractRole(SocketGuildUser userWithRole = null, [Remainder] IRole roleToBeTaken = null){
            var rUser = Context.User as SocketGuildUser;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var Role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == $"{roleToBeTaken}");

            if (rUser.Roles.Contains(directorRole)){

                if (userWithRole == null){
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nYou didn't specify a user; Identify them using their ``Discord ID`` or ``@Mention``.");
                    return;
                }

                if (roleToBeTaken == null){
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nYou did not specify a role.");
                    return;
                } else {
                    var moderatorHierachyPos = rUser.Hierarchy;
                    var roleHierachyPos = Role.Position;

                    if (moderatorHierachyPos > roleHierachyPos){

                        if (!userWithRole.Roles.Contains(Role)){
                            await Context.Message.DeleteAsync();
                            await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                            $"***Uh oh! Something went wrong...***\n\nUser **{userWithRole.Username}** does not possess the **{Role.Name}** role.");
                            return;
                        } else {
                            var embedBuilder = new EmbedBuilder()
                                .WithColor(Color.DarkPurple)
                                .WithDescription($"{rUser.Mention}, user **{userWithRole.Username}'s** {Role.Mention} role has been removed.");
                            Embed embed = embedBuilder.Build();
                            var botReplyFailReason = await ReplyAsync(embed: embed);
                            await userWithRole.RemoveRoleAsync(Role);
                            await Context.Message.DeleteAsync();
                        }
                    } else {
                        await Context.Message.DeleteAsync();
                        await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                        "***Uh oh! Something went wrong...***\n\nYou are not allowed to remove this role; Action denied.");
                    }
                }
            } else {
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou do not have permission to use this.");
            }
        }
    }
}
