using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System.Data;

namespace KatzTheCreator.ModModules{
    public class GiveRole : ModuleBase<SocketCommandContext>{
        [Command("giverole")]
        public async Task AddRole(SocketGuildUser userToBeGivenRole = null, [Remainder] IRole roleToBeGiven = null){
            var rUser = Context.User as SocketGuildUser;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var neaRolePos = Context.Guild.Roles.FirstOrDefault(x => x.Id == 1018285950792642582).Position;
            var Role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == $"{roleToBeGiven}");
            

            if (rUser.Roles.Contains(directorRole)){

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

                } else {
                    var moderatorHierarchyPos = rUser.Hierarchy;
                    var roleHierachyPos = Role.Position;

                    if (moderatorHierarchyPos > roleHierachyPos && neaRolePos > roleHierachyPos){ 

                        if (userToBeGivenRole.Roles.Contains(Role)){
                            await Context.Message.DeleteAsync();
                            await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                            $"***Uh oh! Something went wrong...***\n\nUser **{userToBeGivenRole.Username}** already possesses the **{Role.Name}** role.");
                            return;
                        } else {
                            var embedBuilder = new EmbedBuilder()
                                .WithColor(Color.DarkPurple)
                                .WithDescription($"{rUser.Mention}, **{userToBeGivenRole.Username}** has been given the {Role.Mention} role.");
                            Embed embed = embedBuilder.Build();
                            var botReplyFailReason = await ReplyAsync(embed: embed);
                            await userToBeGivenRole.AddRoleAsync(Role);
                            await Context.Message.DeleteAsync();
                        }
                    } else {
                        await Context.Message.DeleteAsync();
                        await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                        "***Uh oh! Something went wrong...***\n\nYou are not allowed to give this role; Action denied.");
                        return;
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
