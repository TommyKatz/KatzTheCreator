using Discord;
using Discord.Commands;
using Discord.Interactions;

namespace KatzTheCreator.UserModules{
    public class Help : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("help", "need help?")]
        public async Task PingAsync(){
            EmbedBuilder builder = new EmbedBuilder();
            
            builder.AddField("- NeaBot Command Help -", "• *created by suicidekatz* •")
                .AddField("Commands (no permissions needed)",
                "**help** | *Brings you here*\nusage: ``/help``" +
                "\n**av** | *Enlarges the members pfp*\nusage: ``/av 135143527767080960``" +
                "\n**serverinfo** | *Provides info on the current server*\nusage: ``/serverinfo``" +
                "\n**whois** | *Provides info on a certain user*\nusage: ``/whois 135143527767080960``")
                .AddField("Staff Commands (permission dependent)",
                $"\n*For staff related help, please visit the guide channel or contact a supervisor.*")
                .WithColor(Color.DarkPurple)
                .WithThumbnailUrl(Context.Guild.IconUrl);

            Embed embed = builder.Build();

            await RespondAsync(embed: embed);
        }
    }
}

