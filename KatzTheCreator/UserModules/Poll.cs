/*using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using RequireUserPermissionAttribute = Discord.Commands.RequireUserPermissionAttribute;

namespace KatzTheCreator.UserModules{
    public class Poll : InteractionModuleBase<SocketInteractionContext>{
        [SlashCommand("poll", "start a poll for users to vote")]
        [RequireUserPermission(GuildPermission.CreateInstantInvite)]
        public async Task PollVoting([Remainder] string question = null){
            var rUser = Context.User as SocketGuildUser;

            if (string.IsNullOrEmpty(question)){
                await RespondAsync("You didn't ask a question; A question must be provided to use this.", ephemeral: true); ////////
                return;
            } else{
                Emote agreeEmote = Emote.Parse("<:agree:1015291095967600761>");
                Emote disagreeEmote = Emote.Parse("<:disagree:1015291176280141955>");

                List<Emote> emoteToAdd = new List<Emote>();
                emoteToAdd.Add(agreeEmote);
                emoteToAdd.Add(disagreeEmote);
                IEnumerable<Emote> reactionsToAdd = emoteToAdd;

                var embedBuilder = new EmbedBuilder()
                    .WithTitle("Poll Started: react below to submit your stance !")
                    .WithColor(Color.DarkPurple)
                    .WithDescription($"~\n***{question}***\n~")
                    .WithThumbnailUrl("https://i.imgur.com/sGWEXGf.png")
                    .WithCurrentTimestamp()
                    .WithFooter(footer =>{
                        footer
                        .WithText($"asked by {rUser.Username}")
                        .WithIconUrl(rUser.GetAvatarUrl());
                    });
                Embed embed = embedBuilder.Build();
                await RespondAsync(embed: embed);

                // find a way to add reactions to this response
                
            }
        }
    }
}*/