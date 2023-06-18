using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;

namespace KatzTheCreator.UserModules{
    public class WhoIs : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("whois", "provides info on user")]
        public async Task WhoIsUser(SocketGuildUser user =  null){
            var rUser = Context.User as SocketGuildUser;

            if (user == null){
                var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.DarkPurple)
                    .WithAuthor($"{rUser.Username}")
                    .WithTitle("User Information")
                    .WithThumbnailUrl(rUser.GetAvatarUrl())
                    .AddField("ID", $"{rUser.Id}")
                    .AddField("Account Created On", $"{rUser.CreatedAt.UtcDateTime.ToString("D")}")
                    .AddField("Joined Server On", $"{rUser.JoinedAt.Value.UtcDateTime.ToString("D")} ({((int)(DateTime.UtcNow - rUser.JoinedAt.Value.UtcDateTime).TotalDays)} days)")
                    .AddField("Server Roles", $"{string.Join(", ", rUser.Roles)}")
                    .AddField("Status", $"{rUser.Status}");

                Embed embed = embedBuilder.Build();
                await RespondAsync(embed: embed);

            }else{
                var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.DarkPurple)
                    .WithAuthor($"{user.Username}")
                    .WithThumbnailUrl(user.GetAvatarUrl())
                    .AddField("ID", $"{user.Id}")
                    .AddField("Account Created On", $"{user.CreatedAt.UtcDateTime.ToString("D")}")
                    .AddField("Joined Server On", $"{user.JoinedAt.Value.UtcDateTime.ToString("D")} ({((int)(DateTime.UtcNow - user.JoinedAt.Value.UtcDateTime).TotalDays)} days)")
                    .AddField("Server Roles", $"{string.Join(", ", user.Roles)}")
                    .AddField("Status", $"{user.Status}");

                Embed embed = embedBuilder.Build();
                await RespondAsync(embed: embed);
            }
        }
    }
}
