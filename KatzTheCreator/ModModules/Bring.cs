using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.ModModules
{
    public class Bring : ModuleBase<SocketCommandContext>{

        [Command("bring")]
        [Alias("get")]
        public async Task BringUser(SocketGuildUser userToBeBrought = null){

            var rUser = Context.User as SocketGuildUser;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var modRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965726434624688188);
            var waitTimeSeven = 7000;


            if (rUser.Roles.Contains(directorRole) || rUser.Roles.Contains(modRole)){

                if (userToBeBrought == null){
                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithDescription($"{rUser.Mention}, you didn't specify a user; Identify them using their ``Discord ID`` or ``@Mention``.");
                    Embed embed = embedBuilder.Build();
                    var botReplyFailReason = await ReplyAsync(embed: embed);
                    await Context.Message.DeleteAsync();
                    await Task.Delay(waitTimeSeven);
                    await botReplyFailReason.DeleteAsync();
                    return;

                } else if (userToBeBrought.VoiceChannel == null){
                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithDescription($"{rUser.Mention}, the user you are trying to get is not in a voice channel.");
                    Embed embed = embedBuilder.Build();
                    var botReplyFailReason = await ReplyAsync(embed: embed);
                    await Context.Message.DeleteAsync();
                    await Task.Delay(waitTimeSeven);
                    await botReplyFailReason.DeleteAsync();
                    return;

                } else if (rUser.VoiceChannel == null){
                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithDescription($"{rUser.Mention}, you are not in a voice channel; Join one to use this.");
                    Embed embed = embedBuilder.Build();
                    var botReplyFailReason = await ReplyAsync(embed: embed);
                    await Context.Message.DeleteAsync();
                    await Task.Delay(waitTimeSeven);
                    await botReplyFailReason.DeleteAsync();
                    return;
                } else {
                    var rUserVC = rUser.VoiceChannel.Id;
                    await userToBeBrought.ModifyAsync(x => { x.ChannelId = rUserVC; });
                    await Context.Message.DeleteAsync();
                }

            } else {
                var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.DarkPurple)
                    .WithDescription($"{rUser.Mention}, You do not have permission to use this.");
                Embed embed = embedBuilder.Build();
                var botReplyFailPerms = await ReplyAsync(embed: embed);
                await Context.Message.DeleteAsync();
                await Task.Delay(waitTimeSeven);
                await botReplyFailPerms.DeleteAsync();
            }
        }
    }
}
