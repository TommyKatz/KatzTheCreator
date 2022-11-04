using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.UserModules{
    public class WhoIs : ModuleBase<SocketCommandContext> {
        [Command("whois")]
        public async Task WhoIsUser(SocketGuildUser userToBeIDed =  null){

            var rUser = Context.User as SocketGuildUser;

            if (userToBeIDed == null){
                var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.DarkPurple)
                    .WithAuthor($"{rUser}")
                    .WithTitle("User Information")
                    .WithThumbnailUrl(rUser.GetAvatarUrl())
                    .AddField("ID", $"{rUser.Id}")
                    .AddField("Account Created On", $"{rUser.CreatedAt.UtcDateTime.ToString("D")}")
                    .AddField("Joined Server On", $"{rUser.JoinedAt.Value.UtcDateTime.ToString("D")}")
                    .AddField("Server Roles", $"{string.Join(", ", rUser.Roles)}")
                    .AddField("Status", $"{rUser.Status}");

                Embed embed = embedBuilder.Build();
                await ReplyAsync(embed: embed);
                await Context.Message.DeleteAsync();

            } else {

                var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.DarkPurple)
                    .WithAuthor($"{userToBeIDed}")
                    .WithThumbnailUrl(userToBeIDed.GetAvatarUrl())
                    .AddField("ID", $"{userToBeIDed.Id}")
                    .AddField("Account Created On", $"{userToBeIDed.CreatedAt.UtcDateTime.ToString("D")}")
                    .AddField("Joined Server On", $"{userToBeIDed.JoinedAt.Value.UtcDateTime.ToString("D")}")
                    .AddField("Server Roles", $"{string.Join(", ", userToBeIDed.Roles)}")
                    .AddField("Status", $"{userToBeIDed.Status}");

                Embed embed = embedBuilder.Build();
                await ReplyAsync(embed: embed);
                await Context.Message.DeleteAsync();
            }
        }
    }
}
