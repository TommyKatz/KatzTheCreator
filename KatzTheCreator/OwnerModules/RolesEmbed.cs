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
            IEmote scienceEmote = Emote.Parse("<a:SCIENCETEAM:1232231856557129729>");
            IEmote rogueEmote = Emote.Parse("<:ROGUE:1232224653649379378>");
            IEmote usecEmote = Emote.Parse("<:USEC:1117539300167397477>");
            IEmote bearEmote = Emote.Parse("<:BEAR:1117539338020978799>");
            var getSCGRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965700697679077406);
            var getKillerRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965702660542062653);
            var getScientistRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 1232216233483436045);
            var getRogueRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 1232216391143260210);
            var getUsecRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 1117540158112268308);
            var getBearRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 1117540205856038962);
            var rKatz = Context.User as SocketGuildUser;
            var serverName = Context.Guild.Name;
            var serverIconUrl = Context.Guild.IconUrl;

            var builder = new EmbedBuilder()
                .WithColor(Color.DarkPurple)
                .WithThumbnailUrl(serverIconUrl)
                .WithTitle("Survivor? Killer? Both?")
                .WithDescription($"Click any button below to obtain the role(s) you prefer.\n\n~ {getSCGRole.Mention}\n~ {getKillerRole.Mention}");

            Embed embed = builder.Build();

            var builderTwo = new ComponentBuilder()
                .WithButton("Skill Check Gang", "Skill Check Gang", ButtonStyle.Secondary, scgEmote)
                .WithButton("Serial Killer Squad", "Serial Killer Squad", ButtonStyle.Secondary, killerEmote);

            await ReplyAsync(embed: embed, components: builderTwo.Build());

            var builderFive = new EmbedBuilder()
                .WithColor(Color.DarkPurple)
                .WithThumbnailUrl("https://i.imgur.com/Q9WQoR7.png")
                .WithTitle("Among Sus")
                .WithDescription($"Click any button below to obtain the role you prefer.\n\n~ {getScientistRole.Mention}\n~ {getRogueRole.Mention}");
                
            Embed embedFive = builderFive.Build();

            var builderSix = new ComponentBuilder()
                .WithButton("Science Team", "Science Team", ButtonStyle.Secondary, scienceEmote)
                .WithButton("Rogue Scientist", "Rogue Scientist", ButtonStyle.Secondary, rogueEmote);

            await ReplyAsync(embed: embedFive, components: builderSix.Build());

            var builderThree = new EmbedBuilder()
                .WithColor(Color.DarkPurple)
                .WithTitle("Do you play Tarkov? Which PMC are you?")
                .WithDescription($"Click any button below to obtain the role you prefer.\n\n~ {getUsecRole.Mention}\n~ {getBearRole.Mention}")
                .WithThumbnailUrl("https://i.imgur.com/jWDQuTf.png")
                .WithFooter(footer => {
                    footer
                    .WithText($" {serverName}  |  Nea Bot  •  developed & maintained by antekatz")
                    .WithIconUrl(rKatz.GetAvatarUrl());
                });

            Embed embedThree = builderThree.Build();

            var builderFour = new ComponentBuilder()
                .WithButton("USEC", "USEC", ButtonStyle.Secondary, usecEmote)
                .WithButton("BEAR", "BEAR", ButtonStyle.Secondary, bearEmote);

            await Context.Message.DeleteAsync();
            await ReplyAsync(embed: embedThree, components: builderFour.Build());
        }
    }
}

