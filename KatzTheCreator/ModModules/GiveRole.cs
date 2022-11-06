using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RequireUserPermissionAttribute = Discord.Commands.RequireUserPermissionAttribute;

namespace KatzTheCreator.ModModules{
    public class GiveRole : ModuleBase<SocketCommandContext>{
        [Command("giverole")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddRole(SocketGuildUser userToBeGivenRole = null, [Remainder] IRole roleToBeGiven = null){
            var rUser = Context.User as SocketGuildUser;

            if (userToBeGivenRole == null){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou didn't specify a user; Identify them using their ``Discord ID`` or ``@Mention``.");
                return;
            }

            if (roleToBeGiven == null){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou didn't specify a role.");
                return;

            }else{
                var Role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == $"{roleToBeGiven}");
                var moderatorHierarchyPos = rUser.Hierarchy;
                var roleHierachyPos = Role.Position;
                var neaRolePos = Context.Guild.Roles.FirstOrDefault(x => x.Id == 1018285950792642582).Position;

                if (moderatorHierarchyPos > roleHierachyPos && neaRolePos > roleHierachyPos){

                    if (userToBeGivenRole.Roles.Contains(Role)){
                        await Context.Message.DeleteAsync();
                        await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                        $"***Uh oh! Something went wrong...***\n\nUser **{userToBeGivenRole.Username}** already possesses the **{Role.Name}** role.");
                        return;
                    }else{
                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{rUser.Mention}, **{userToBeGivenRole.Username}** has been given the {Role.Mention} role.");
                        Embed embed = embedBuilder.Build();
                        var botReplyFailReason = await ReplyAsync(embed: embed);
                        await userToBeGivenRole.AddRoleAsync(Role);
                        await Context.Message.DeleteAsync();
                    }
                }else{
                    await Context.Message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    "***Uh oh! Something went wrong...***\n\nYou are not allowed to give this role; Action denied.");
                    return;
                }
            }
        }
    }
}
