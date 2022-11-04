using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.ModModules
{
    public class Poll : ModuleBase<SocketCommandContext>{

        [Command("poll")]
        public async Task PollVoting([Remainder] string question){
            var rUser = Context.User as SocketGuildUser;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);
            var modRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965726434624688188);
            var repRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 1009266690204385322);
            Emote agreeEmote = Emote.Parse("<:agree:1015291095967600761>");
            Emote disagreeEmote = Emote.Parse("<:disagree:1015291176280141955>");

            List<Emote> emoteToAdd = new List<Emote>();
            emoteToAdd.Add(agreeEmote);
            emoteToAdd.Add(disagreeEmote);
            IEnumerable<Emote> reactionsToAdd = emoteToAdd;

            var embedBuilder = new EmbedBuilder()
                .WithTitle("Poll Started: react below !")
                .WithColor(Color.DarkPurple)
                .WithDescription($"***{question}***")
                .WithFooter(footer => {
                    footer
                    .WithText($"asked by {rUser}")
                    .WithIconUrl(rUser.GetAvatarUrl());
                });
            Embed embed = embedBuilder.Build();
            var botReply = await ReplyAsync(embed: embed);
            await Context.Message.DeleteAsync();
            await botReply.AddReactionsAsync(reactionsToAdd);
        }
    }
}
