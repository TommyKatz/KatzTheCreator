using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.ModModules{
    public class RolesEmbed : ModuleBase<SocketCommandContext>{
        [Command("RolesEmbedPerm")]
        [RequireOwner]
        public async Task ReactionsEmbed(){

            IEmote scgEmote = Emote.Parse("<:SCGMilk:992218646124445836>");
            IEmote killerEmote = Emote.Parse("<:killer:1014549177369374740>");
            var getSCGRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965700697679077406);
            var getKillerRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965702660542062653);
            var rKatz = Context.User as SocketGuildUser;

            var serverIconUrl = Context.Guild.IconUrl;

            var builder = new EmbedBuilder()
                .WithColor(Color.DarkPurple)
                .WithThumbnailUrl(serverIconUrl)
                .WithTitle("Survivor? Killer? Both?")
                .WithDescription($"Click any button below to obtain the role(s) you prefer.\n\n~ {getSCGRole.Mention}\n~ {getKillerRole.Mention}")
                .WithFooter(footer => {
                    footer
                    .WithText($" Bugs By Daylight  |  Nea Bot  •  Created and Developed by katz#9999")
                    .WithIconUrl(rKatz.GetAvatarUrl());
                });
            Embed embed = builder.Build();

            var builderTwo = new ComponentBuilder()
                .WithButton("Skill Check Gang", "Skill Check Gang", ButtonStyle.Secondary, scgEmote)
                .WithButton("Serial Killer Squad", "Serial Killer Squad", ButtonStyle.Secondary, killerEmote);

            await Context.Message.DeleteAsync();
            await ReplyAsync(embed: embed, components: builderTwo.Build());
        }
    }
}

