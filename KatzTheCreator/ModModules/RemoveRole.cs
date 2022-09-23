using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatzTheCreator.ModModules{
    public class RemoveRole : ModuleBase<SocketCommandContext>{
        [Command("removerole")]
        public async Task SubtractRole(SocketGuildUser userWithRole = null, [Remainder] IRole roleToBeTaken = null){
            var rUser = Context.User as SocketGuildUser;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var Role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == $"{roleToBeTaken}");
            var waitTimeSeven = 7000;

            if (rUser.Roles.Contains(directorRole)){

                if (userWithRole == null){
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

                if (roleToBeTaken == null){
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

                        if (!userWithRole.Roles.Contains(Role)){

                            var embedBuilder = new EmbedBuilder()
                                .WithColor(Color.DarkPurple)
                                .WithDescription($"{rUser.Mention}, user **{userWithRole.Username}** does not possess the {Role.Mention} role.");
                            Embed embed = embedBuilder.Build();
                            var botReplyFailReason = await ReplyAsync(embed: embed);
                            await Context.Message.DeleteAsync();
                            await Task.Delay(waitTimeSeven);
                            await botReplyFailReason.DeleteAsync();
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
                        var amountBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithDescription($"{rUser.Mention}, you are not allowed to remove this role; Action denied.");
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
