using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.OwnerModules
{
    public class Giveaway : ModuleBase<SocketCommandContext>
    {
        [Command("newga")]
        [RequireOwner]
        public async Task GiveawayEmbed(string pictureLink = null, [Remainder] string giveawayContent = null)
        {

            var rUser = Context.User as SocketGuildUser;

            if (string.IsNullOrEmpty(pictureLink) || !pictureLink.StartsWith("https://"))
            {
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nLink is null or incorrect format; **Copy image address**.");
                return;
            }
            else if (string.IsNullOrEmpty(giveawayContent))
            {
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou didn't specify the content; Content must be provided to use this.");
                return;
            }
            else if (pictureLink.EndsWith(".png") || pictureLink.EndsWith(".jpg"))
            {
                Emote joinEmote = Emote.Parse("<a:join:993953953832251542>");
                Emote leftDownEmote = Emote.Parse("<:downleft:1039620899894218763>");
                Emote rightDownEmote = Emote.Parse("<:downright:1039620982790422658>");

                await Context.Message.DeleteAsync();
                var embedBuilder = new EmbedBuilder()
                    .WithTitle("ATTENTION: Giveaway Time !")
                    .WithColor(Color.DarkMagenta)
                    .WithDescription($"{leftDownEmote} React to the emoji below to enter {rightDownEmote}")
                    .AddField($"Content in Giveaway:", $"{giveawayContent}")
                    .WithImageUrl($"{pictureLink}")
                    .WithCurrentTimestamp()
                    .WithFooter(footer =>
                    {
                        footer
                        .WithText($"Hosted by {rUser}")
                        .WithIconUrl(rUser.GetAvatarUrl());
                    });
                Embed embed = embedBuilder.Build();
                var botReply = await ReplyAsync(embed: embed);
                await botReply.AddReactionAsync(joinEmote);
            }
            else
            {
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nIncorrect format, link needs to end with .png or .jpg; **Copy image address**.");
                return;
            }
        }
    }
}
