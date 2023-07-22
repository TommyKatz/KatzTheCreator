using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;

namespace KatzTheCreator.UserModules{
    public class Report : InteractionModuleBase<SocketInteractionContext>{

        [SlashCommand("report", "submit a report to a staff member")]
        public async Task ReportTask([Remainder]string statement){
            var reportsChannel = Context.Guild.GetChannel(1126172186156212374) as SocketTextChannel;
            var userReporting = Context.User;

            await RespondAsync("Thank you for the report, a staff member will be with you shortly. Ensure DMs are enabled for this server.", ephemeral: true);

            var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.DarkMagenta)
                    .WithAuthor($"{userReporting.Username} (ID: {userReporting.Id})", userReporting.GetAvatarUrl())
                    .WithThumbnailUrl("https://i.imgur.com/YGpG0t2.png")
                    .WithDescription($"{statement}")
                    .WithCurrentTimestamp();

            var embed = embedBuilder.Build();

            var builderTwo = new ComponentBuilder()
                .WithButton("Claim", "Claim", ButtonStyle.Success);

            await reportsChannel.SendMessageAsync(embed: embed, components: builderTwo.Build());
        }

    }
}
