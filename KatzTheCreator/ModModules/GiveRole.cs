using Discord.Commands;
using Discord.WebSocket;
using Discord;

namespace KatzTheCreator.ModModules{
    public class GiveRole : ModuleBase<SocketCommandContext>{
        [Command("giverole")]
        public async Task AddRole(SocketGuildUser userToBeGivenRole = null, [Remainder] IRole roleToBeGiven = null){
            var rUser = Context.User as SocketGuildUser;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var Role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == $"{roleToBeGiven}");
            var waitTimeSeven = 7000;

            if (rUser.Roles.Contains(directorRole)){

                if (userToBeGivenRole == null){
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

                if (roleToBeGiven == null){
                    var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{rUser.Mention}, you did not specify a role.");
                    Embed embed = embedBuilder.Build();
                    var botReplyFailUser = await ReplyAsync(embed: embed);
                    await Context.Message.DeleteAsync();
                    await Task.Delay(waitTimeSeven);
                    await botReplyFailUser.DeleteAsync();
                    return;

                } else {
                    var moderatorHierachyPos = rUser.Hierarchy;
                    var roleHierachyPos = Role.Position;

                    if (moderatorHierachyPos > roleHierachyPos){ 

                        if (userToBeGivenRole.Roles.Contains(Role)){

                            var embedBuilder = new EmbedBuilder()
                                .WithColor(Color.DarkPurple)
                                .WithDescription($"{rUser.Mention}, user **{userToBeGivenRole.Username}** already possesses the {Role.Mention} role.");
                            Embed embed = embedBuilder.Build();
                            var botReplyFailUser = await ReplyAsync(embed: embed);
                            await Context.Message.DeleteAsync();
                            await Task.Delay(waitTimeSeven);
                            await botReplyFailUser.DeleteAsync();
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
                        var amountBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithDescription($"{rUser.Mention}, you are not allowed to give this role; Action denied.");
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
