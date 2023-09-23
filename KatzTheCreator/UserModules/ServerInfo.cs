using Discord;
using Discord.Commands;
using Discord.Interactions;

namespace KatzTheCreator.UserModules
{
    public class ServerInfo : InteractionModuleBase<SocketInteractionContext>{
        [SlashCommand("serverinfo", "provides info about the server")]
        public async Task PingAsync(){
            var boostCount = Context.Guild.PremiumSubscriptionCount;
        
            if (boostCount < 14){
                EmbedBuilder builder = new EmbedBuilder();

                builder.WithTitle($"{Context.Guild.Name}")
                    .WithColor(Color.DarkPurple)
                    .WithThumbnailUrl(Context.Guild.IconUrl)
                    .AddField("Owner", $"<@507965773818494996> *(ID: 507965773818494996)*") // {Context.Guild.Owner.Mention} *(ID: {Context.Guild.OwnerId})*
                    .AddField("Date Created", $"{Context.Guild.CreatedAt.UtcDateTime.ToString("D")}")
                    .AddField("Member Count", $"{Context.Guild.MemberCount}")
                    .AddField("Server Booster Count", $"{boostCount} boosts")
                    .AddField("Server Level", $"{Context.Guild.PremiumTier}, {14 - boostCount} boosts away from Tier3 !")
                    .AddField("Role Count", $"{Context.Guild.Roles.Count}")
                    .WithFooter($"For more information, contact {Context.Guild.Owner.Username}")
                    .WithCurrentTimestamp();

                await RespondAsync(embed: builder.Build());

            }else if (boostCount == 14){
                EmbedBuilder builder = new EmbedBuilder();

                builder.WithTitle($"{Context.Guild.Name}")
                    .WithColor(Color.DarkPurple)
                    .WithThumbnailUrl(Context.Guild.IconUrl)
                    .AddField("Owner", $"<@507965773818494996> *(ID: 507965773818494996)*")
                    .AddField("Date Created", $"{Context.Guild.CreatedAt.UtcDateTime.ToString("D")}")
                    .AddField("Member Count", $"{Context.Guild.MemberCount}")
                    .AddField("Server Booster Count", $"{boostCount} boosts")
                    .AddField("Server Level", $"{Context.Guild.PremiumTier}, no more boosts needed but are still greatly appreciated !")
                    .AddField("Role Count", $"{Context.Guild.Roles.Count}")
                    .WithFooter($"For more information, contact {Context.Guild.Owner.Username}")
                    .WithCurrentTimestamp();

                await RespondAsync(embed: builder.Build());

            }
            else{
                EmbedBuilder builder = new EmbedBuilder();

                builder.WithTitle($"{Context.Guild.Name}")
                    .WithColor(Color.DarkPurple)
                    .WithThumbnailUrl(Context.Guild.IconUrl)
                    .AddField("Owner", $"<@507965773818494996> *(ID: 507965773818494996)*")
                    .AddField("Date Created", $"{Context.Guild.CreatedAt.UtcDateTime.ToString("D")}")
                    .AddField("Member Count", $"{Context.Guild.MemberCount}")
                    .AddField("Server Booster Count", $"{boostCount} boosts")
                    .AddField("Server Level", $"{Context.Guild.PremiumTier}, {boostCount - 14} boosts over required amount for Tier3 !")
                    .AddField("Role Count", $"{Context.Guild.Roles.Count}")
                    .WithFooter($"For more information, contact {Context.Guild.Owner.Username}")
                    .WithCurrentTimestamp();

                await RespondAsync(embed: builder.Build());
            }
        }
    }
}
