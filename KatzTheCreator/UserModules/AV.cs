using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;

namespace KatzTheCreator.UserModules{
    public class AV : InteractionModuleBase<SocketInteractionContext>{
        [SlashCommand("av", "grabs and uploads pfps")]
        public async Task AVUser (SocketGuildUser user = null){
            var rUser = Context.User as SocketGuildUser;

            if (user == null){

                var embedBuilder = new EmbedBuilder()
                    .WithAuthor(rUser.Username)
                    .WithColor(Color.DarkPurple)
                    .WithTitle("Avatar")
                    .WithImageUrl(rUser.GetAvatarUrl(ImageFormat.Auto, 320))
                    .WithFooter($"Requested by {rUser.Username}");

                Embed embed = embedBuilder.Build();
                await RespondAsync(embed: embed);

            }else{
                var embedBuilder = new EmbedBuilder()
                    .WithAuthor(user.Username)
                    .WithColor(Color.DarkPurple)
                    .WithTitle("Avatar")
                    .WithImageUrl(user.GetAvatarUrl(ImageFormat.Auto, 320))
                    .WithFooter($"Requested by {rUser.Username}");

                Embed embed = embedBuilder.Build();
                await RespondAsync(embed: embed);
            }

            
        }
    }
}
