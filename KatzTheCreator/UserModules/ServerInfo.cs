﻿using Discord.Commands;
using Discord;

namespace KatzTheCreator.UserModules{
    public class ServerInfo : ModuleBase<SocketCommandContext>{
        [Command("serverinfo")]
        public async Task PingAsync(){

            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"{Context.Guild.Name}")
                .WithColor(Color.DarkPurple)
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .AddField("Owner", $"{Context.Guild.Owner.Mention} *(ID: {Context.Guild.OwnerId})*")
                .AddField("Date Created", $"{Context.Guild.CreatedAt.UtcDateTime.ToString("D")}")
                .AddField("Member Count", $"{Context.Guild.MemberCount}")
                .AddField("Server Booster Count", $"{Context.Guild.PremiumSubscriptionCount} boosts")
                .AddField("Server Level", $"{Context.Guild.PremiumTier}, {14 - Context.Guild.PremiumSubscriptionCount} boosts away from Tier3 !")
                .AddField("Role Count", $"{Context.Guild.Roles.Count}")
                .WithFooter($"For more information, contact {Context.Guild.Owner}")
                .WithCurrentTimestamp();

            await ReplyAsync("", false, builder.Build());
            await Context.Message.DeleteAsync();
        }
    }
}