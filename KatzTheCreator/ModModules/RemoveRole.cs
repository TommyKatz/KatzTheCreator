using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RequireUserPermissionAttribute = Discord.Commands.RequireUserPermissionAttribute;

namespace KatzTheCreator.ModModules{
    public class RemoveRole : ModuleBase<SocketCommandContext>{
        [Command("removerole")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SubtractRole(SocketGuildUser userWithRole = null, [Remainder] IRole roleToBeTaken = null)
        {
            var rUser = Context.User as SocketGuildUser;

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
            }else{
                var moderatorHierachyPos = rUser.Hierarchy;
                var Role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == $"{roleToBeTaken}");
                var roleHierachyPos = Role.Position;

                if (moderatorHierachyPos > roleHierachyPos){

                    if (moderatorHierachyPos > userWithRole.Hierarchy){

                        if (!userWithRole.Roles.Contains(Role)){
                            await Context.Message.DeleteAsync();
                            await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                            $"***Uh oh! Something went wrong...***\n\nUser **{userWithRole.Username}** does not possess the **{Role.Name}** role.");
                            return;
                        }else{
                            var embedBuilder = new EmbedBuilder()
                                .WithColor(Color.DarkPurple)
                                .WithDescription($"{rUser.Mention}, user **{userWithRole.Username}'s** {Role.Mention} role has been removed.");
                            Embed embed = embedBuilder.Build();
                            var botReplyFailReason = await ReplyAsync(embed: embed);
                            await userWithRole.RemoveRoleAsync(Role);
                            await Context.Message.DeleteAsync();
                        }
                    }else{
                        await Context.Message.DeleteAsync();
                        await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                        "***Uh oh! Something went wrong...***\n\nYou are not allowed to remove this role; Action denied.");
                    }
                }else{
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nYou are not allowed to remove this role; Action denied.");
                }
            }
        }
    }
}
