using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.ModModules
{
    public class ShrineAlerts : ModuleBase<SocketCommandContext>{
        [Command("newshrine")]
        public async Task ShrineEmbed(string pictureUrl = null, [Remainder] string content = null){

            var rUser = Context.User as SocketGuildUser;
            var announceChannel = Context.Guild.GetChannel(988184539455172688) as SocketTextChannel;

            List<ulong> discordId = new List<ulong>();
            discordId.Add(135143527767080960); // katz
            //discordId.Add(527473869439762434); // harpy
            IEnumerable<ulong> allowedIds = discordId;

            if (!allowedIds.Contains(rUser.Id)){
                await Context.Message.DeleteAsync();
                return;
            }

            if (string.IsNullOrEmpty(pictureUrl) || !pictureUrl.StartsWith("https://")){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nLink is null or incorrect format; **Copy image address**.");
                return;
            }else if (string.IsNullOrEmpty(content)){
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nYou didn't specify the content; Content must be provided to use this.");
                return;
            }else if (pictureUrl.EndsWith(".png") || pictureUrl.EndsWith(".jpg")){

                await Context.Message.DeleteAsync();
                await announceChannel.SendMessageAsync("<@&1036799014508703794>");

                var embedBuilder = new EmbedBuilder()
                    .WithTitle("New Shrine of Secrets !")
                    .WithColor(Color.DarkGreen)
                    .AddField($"Added Perks:", $"{string.Join(", ", content)}")
                    .WithImageUrl($"{pictureUrl}")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithText($"Published by {rUser}")
                        .WithIconUrl(rUser.GetAvatarUrl());
                    });
                Embed embed = embedBuilder.Build();

                await announceChannel.SendMessageAsync(embed: embed);

            } else {
                await Context.Message.DeleteAsync();
                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                "***Uh oh! Something went wrong...***\n\nIncorrect format, link needs to end with .png or .jpg; **Copy image address**.");
                return;
            }
        }

    }
}
