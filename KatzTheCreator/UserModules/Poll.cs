using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RequireUserPermissionAttribute = Discord.Commands.RequireUserPermissionAttribute;

namespace KatzTheCreator.UserModules{
    public class Poll : ModuleBase<SocketCommandContext>{
        [Command("poll")]
        [RequireUserPermission(GuildPermission.CreateInstantInvite)]
        public async Task PollVoting([Remainder] string question = null){
            var rUser = Context.User as SocketGuildUser;

            if (string.IsNullOrEmpty(question)){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou didn't ask a question; A question must be provided to use this.");
                return;
            } else{
                Emote agreeEmote = Emote.Parse("<:agree:1015291095967600761>");
                Emote disagreeEmote = Emote.Parse("<:disagree:1015291176280141955>");

                List<Emote> emoteToAdd = new List<Emote>();
                emoteToAdd.Add(agreeEmote);
                emoteToAdd.Add(disagreeEmote);
                IEnumerable<Emote> reactionsToAdd = emoteToAdd;

                await Context.Message.DeleteAsync();
                var embedBuilder = new EmbedBuilder()
                    .WithTitle("Poll Started: react below to submit your stance !")
                    .WithColor(Color.DarkPurple)
                    .WithDescription($"~\n***{question}***\n~")
                    .WithThumbnailUrl("https://i.imgur.com/sGWEXGf.png")
                    .WithCurrentTimestamp()
                    .WithFooter(footer =>{
                        footer
                        .WithText($"asked by {rUser}")
                        .WithIconUrl(rUser.GetAvatarUrl());
                    });
                Embed embed = embedBuilder.Build();
                var botReply = await ReplyAsync(embed: embed);
                await botReply.AddReactionsAsync(reactionsToAdd);
            }
        }
    }
}
