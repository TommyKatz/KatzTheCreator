using Discord;
using Discord.Commands;

namespace KatzTheCreator.OwnerModules
{
    public class CodesEmbed : ModuleBase<SocketCommandContext>
    {
        [Command("CodesEmbedPerm")]
        [RequireOwner]
        public async Task ReactionsEmbed()
        {
            var getAlertsRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 1036799014508703794);

            var builder = new EmbedBuilder()
                .WithColor(Color.DarkMagenta)
                .WithTitle("Information | Self Roles")
                .WithDescription($"Click any button below to obtain the role(s) you prefer. \n\n~ {getAlertsRole.Mention} - Get pings for new DBD Codes");
            Embed embed = builder.Build();

            var builderTwo = new ComponentBuilder()
                .WithButton("Code Alerts", "Code Alerts", ButtonStyle.Secondary);

            await Context.Message.DeleteAsync();
            await ReplyAsync(embed: embed, components: builderTwo.Build());
        }


    }
}
