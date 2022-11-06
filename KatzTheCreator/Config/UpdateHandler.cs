using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

namespace KatzTheCreator.Config{
    public class UpdateHandler{
        
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public UpdateHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services){

            _commands = commands;
            _client = client;
            _services = services;

            _client.MessageReceived += MessageOpsAsync;
            _client.ButtonExecuted += RegisterButtonHandler;
            _client.UserBanned += UserBannedLog;
            _client.UserUnbanned += UserUnbannedLog;
            _client.UserJoined += AnnounceJoinedUser;
            _client.UserJoined += JoinLogging;
            _client.UserLeft += LeaveLogging;
            _client.GuildMemberUpdated += UserUpdatedOps;
        }

        public async Task RegisterCommandAsync(){
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task UserUpdatedOps(Cacheable<SocketGuildUser, ulong> stateBefore, SocketGuildUser userStateAfter){
            var userStateBefore = stateBefore.Value;
            var boostRole = userStateAfter.Guild.Roles.FirstOrDefault(x => x.Id == 968257372973826088);
            var loggingChannel = _client.GetChannel(965699174358216744) as SocketTextChannel;

            if (userStateAfter.TimedOutUntil.HasValue){
                var currentGuild = userStateAfter.Guild as IGuild;
                var timeOutAuthor = (await currentGuild.GetAuditLogsAsync(actionType: ActionType.MemberUpdated)).FirstOrDefault().User;
                var timoutReason = (await currentGuild.GetAuditLogsAsync(actionType: ActionType.MemberUpdated)).FirstOrDefault().Reason;

                if (!string.IsNullOrEmpty(timoutReason)){
                    try{
                        var builder = new EmbedBuilder()
                            .WithColor(Color.Gold)
                            .WithThumbnailUrl(userStateAfter.GetAvatarUrl())
                            .WithAuthor($"{timeOutAuthor} (ID: {timeOutAuthor.Id})", timeOutAuthor.GetAvatarUrl())
                            .WithDescription($"**Timed Out:** {userStateAfter.Mention} *(ID: {userStateAfter.Id})*\n**Reason:** {timoutReason}")
                            .WithCurrentTimestamp();
                        Embed embedTwo = builder.Build();
                        await loggingChannel.SendMessageAsync(embed: embedTwo);
                    } catch (Exception){
                        await loggingChannel.SendMessageAsync($"{timeOutAuthor.Mention}, an exception has been caught, please contact @katz#9999.");
                    }
                } else{
                    try{
                        var builder = new EmbedBuilder()
                            .WithColor(Color.Gold)
                            .WithThumbnailUrl(userStateAfter.GetAvatarUrl())
                            .WithAuthor($"{timeOutAuthor} (ID: {timeOutAuthor.Id})", timeOutAuthor.GetAvatarUrl())
                            .WithDescription($"**Timed Out:** {userStateAfter.Mention} *(ID: {userStateAfter.Id})*\n**Reason:** No Reason Provided.")
                            .WithCurrentTimestamp();
                        Embed embedTwo = builder.Build();
                        await loggingChannel.SendMessageAsync(embed: embedTwo);
                    }catch (Exception){
                        await loggingChannel.SendMessageAsync($"{timeOutAuthor.Mention}, an exception has been caught, please contact @katz#9999.");
                    }
                }
            }
            
            if (userStateBefore.TimedOutUntil.HasValue && !userStateAfter.TimedOutUntil.HasValue){
                var currentGuild = userStateAfter.Guild as IGuild;
                var timeOutAuthor = (await currentGuild.GetAuditLogsAsync(actionType: ActionType.MemberUpdated)).FirstOrDefault().User;
                try{
                    var builder = new EmbedBuilder()
                        .WithColor(Color.Gold)
                        .WithThumbnailUrl(userStateAfter.GetAvatarUrl())
                        .WithAuthor($"{timeOutAuthor} (ID: {timeOutAuthor.Id})", timeOutAuthor.GetAvatarUrl())
                        .WithDescription($"**Timed Out Removed:** {userStateAfter.Mention} *(ID: {userStateAfter.Id})*\n**Reason cannot be provided.**")
                        .WithCurrentTimestamp();
                    Embed embedTwo = builder.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);
                }catch (Exception){
                    await loggingChannel.SendMessageAsync($"{timeOutAuthor.Mention}, an exception has been caught, please contact @katz#9999.");
                }
            }

            if (!userStateBefore.Roles.Contains(boostRole) && userStateAfter.Roles.Contains(boostRole)){
                var boostAnnounceChannel = _client.GetChannel(960957925143224343) as SocketTextChannel;
                var boostCount = userStateAfter.Guild.PremiumSubscriptionCount;
                var server = _client.GetGuild(960957925143224340);

                var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.Purple)
                    .WithAuthor($"{userStateAfter}", userStateAfter.GetAvatarUrl())
                    .WithThumbnailUrl("https://i.imgur.com/i0xdsgG.png")
                    .WithDescription($"**Thank you for boosting {server} !**\n" +
                    $"**We are now at {boostCount} boosts.**\n◇────────────────────────────◇");
                Embed embed = embedBuilder.Build();

                await boostAnnounceChannel.SendMessageAsync($"{userStateAfter.Mention}");
                await boostAnnounceChannel.SendMessageAsync(embed: embed);
            }
        }

        public async Task UserBannedLog(IUser userBanned, IGuild currentGuild){

            var banAuthor = (await currentGuild.GetAuditLogsAsync(actionType: ActionType.Ban)).FirstOrDefault().User;
            var banReason = (await currentGuild.GetBanAsync(userBanned)).Reason;
            var loggingChannel = _client.GetChannel(965699174358216744) as SocketTextChannel;

            if (!banAuthor.IsBot){

                if (!string.IsNullOrEmpty(banReason)){
                    try{
                        var builderTwo = new EmbedBuilder()
                            .WithColor(Color.DarkRed)
                            .WithThumbnailUrl(userBanned.GetAvatarUrl())
                            .WithAuthor($"{banAuthor} (ID: {banAuthor.Id})", banAuthor.GetAvatarUrl())
                            .WithDescription($"**Banned:** {userBanned.Mention} *(ID: {userBanned.Id})*\n**Reason:** {banReason}")
                            .WithCurrentTimestamp()
                            .WithFooter(footer =>{
                            footer
                            .WithText("Right Click");
                            });
                        Embed embedTwo = builderTwo.Build();
                        await loggingChannel.SendMessageAsync(embed: embedTwo);
                    } catch (Exception){
                        await loggingChannel.SendMessageAsync($"{banAuthor.Mention}, an exception has been caught, please contact @katz#9999.");
                    }

                }else{
                    try{
                        var builderTwo = new EmbedBuilder()
                            .WithColor(Color.DarkRed)
                            .WithThumbnailUrl(userBanned.GetAvatarUrl())
                            .WithAuthor($"{banAuthor} (ID: {banAuthor.Id})", banAuthor.GetAvatarUrl())
                            .WithDescription($"**Banned:** {userBanned.Mention} *(ID: {userBanned.Id})*\n**Reason:** No Reason Provided")
                            .WithCurrentTimestamp()
                            .WithFooter(footer =>{
                            footer
                            .WithText("Right Click");
                            });
                        Embed embedTwo = builderTwo.Build();
                        await loggingChannel.SendMessageAsync(embed: embedTwo);
                    } catch (Exception){
                        await loggingChannel.SendMessageAsync($"{banAuthor.Mention}, an exception has been caught, please contact @katz#9999.");
                    }
                }
            }
        }

        public async Task UserUnbannedLog(IUser userUnbanned, IGuild thisGuild){
            var currentGuild = thisGuild as IGuild;
            var unbanAuthor = (await currentGuild.GetAuditLogsAsync(actionType: ActionType.Unban)).FirstOrDefault().User;
            var loggingChannel = _client.GetChannel(965699174358216744) as SocketTextChannel;

            if (!unbanAuthor.IsBot){
                try{
                    var builderTwo = new EmbedBuilder()
                    .WithColor(Color.DarkGreen)
                    .WithThumbnailUrl(userUnbanned.GetAvatarUrl())
                    .WithAuthor($"{unbanAuthor} (ID: {unbanAuthor.Id})", unbanAuthor.GetAvatarUrl())
                    .WithDescription($"**Unbanned:** {userUnbanned.Mention} *(ID: {userUnbanned.Id})*\n**Reason:** No Reason Provided")
                    .WithCurrentTimestamp()
                    .WithFooter(footer =>{
                        footer
                        .WithText("Right Click");
                    });
                    Embed embedTwo = builderTwo.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);
                } catch{
                    await loggingChannel.SendMessageAsync($"{unbanAuthor.Mention}, an exception has been caught, please contact @katz#9999.");
                }
            }
        }

        public async Task RegisterButtonHandler(SocketMessageComponent component){
            var rUser = component.User as SocketGuildUser;
            ulong scgRoleID = 965700697679077406;
            ulong killerRoleID = 965702660542062653;
            ulong alertsRoleID = 1036799014508703794;
            var getSCGRole = rUser.Guild.Roles.FirstOrDefault(x => x.Id == 965700697679077406);
            var getKillerRole = rUser.Guild.Roles.FirstOrDefault(x => x.Id == 965702660542062653);
            var getAlertsRole = rUser.Guild.Roles.FirstOrDefault(x => x.Id == 1036799014508703794);

            switch (component.Data.CustomId){

                case "Skill Check Gang":

                    if (!rUser.Roles.Contains(getSCGRole)){
                        await rUser.AddRoleAsync(scgRoleID);
                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{component.User.Mention}, i assigned you the {getSCGRole.Mention} role.");
                        Embed embed = embedBuilder.Build();
                        await component.RespondAsync(embed: embed, ephemeral: true);

                    } else {
                        await rUser.RemoveRoleAsync(scgRoleID);
                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{component.User.Mention}, i removed your {getSCGRole.Mention} role.");
                        Embed embed = embedBuilder.Build();
                        await component.RespondAsync(embed: embed, ephemeral: true);
                    }
                    break;

                case "Serial Killer Squad":

                    if (!rUser.Roles.Contains(getKillerRole)){
                        await rUser.AddRoleAsync(killerRoleID);
                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{component.User.Mention}, i assigned you the {getKillerRole.Mention} role.");
                        Embed embed = embedBuilder.Build();
                        await component.RespondAsync(embed: embed, ephemeral: true);
                    } else {
                        await rUser.RemoveRoleAsync(killerRoleID);
                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{component.User.Mention}, i removed your {getKillerRole.Mention} role.");
                        Embed embed = embedBuilder.Build();
                        await component.RespondAsync(embed: embed, ephemeral: true);
                    }
                    break;

                case "Code Alerts":

                    if (!rUser.Roles.Contains(getAlertsRole)){
                        await rUser.AddRoleAsync(alertsRoleID);
                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{component.User.Mention}, i assigned you the {getAlertsRole.Mention} role.");
                        Embed embed = embedBuilder.Build();
                        await component.RespondAsync(embed: embed, ephemeral: true);
                    } else {
                        await rUser.RemoveRoleAsync(alertsRoleID);
                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{component.User.Mention}, i removed your {getAlertsRole.Mention} role.");
                        Embed embed = embedBuilder.Build();
                        await component.RespondAsync(embed: embed, ephemeral: true);
                    }
                    break;
            }
        }

        public async Task MessageOpsAsync(SocketMessage msg){
            if (msg.Author.IsBot) return;

            Emote hmmEmote = Emote.Parse("<:hmm:1025075030540959775>");
            Random rnd = new Random();
            int number = rnd.Next(1, 101);

            if (number <= 4){ // 4% chance
                await msg.AddReactionAsync(hmmEmote);
            }

            // under construction 
            // flags a channel with no messages as spam
            /*try{
                var socketMessage = msg as SocketUserMessage;
                var userSpamming = socketMessage.Author as SocketGuildUser;
                var messages = await msg.Channel.GetMessagesAsync(5).FlattenAsync();

                //bool messagesComparedMatch = messages.All(x => x.Content == socketMessage.Content); // Compares messages to see if they match
                bool messageAuthorsMatch = messages.All(a => a.Author == socketMessage.Author);
                var newSpamTime = socketMessage.Timestamp.DateTime;
                var oldSpamTime = messages.Last().Timestamp.DateTime;
                var bugsServer = _client.GetGuild(960957925143224340);
                var mutedRole = bugsServer.Roles.FirstOrDefault(x => x.Id == 966087394040369182);
                var rcsLogChannel = _client.GetChannel(1027256422095925338) as SocketTextChannel;
                var timeResult = (newSpamTime - oldSpamTime).TotalSeconds;

                if ((newSpamTime - oldSpamTime).TotalSeconds <= 5 && messageAuthorsMatch && messages.All(5))
                {
                    await userSpamming.AddRoleAsync(mutedRole);
                    await ((ITextChannel)socketMessage.Channel).DeleteMessagesAsync(messages);

                    var embedBuilder = new EmbedBuilder()
                        .WithTitle("RCS ALERT: Spam Detection")
                        .WithColor(Color.DarkRed)
                        .WithDescription($"***Presumed Suspect:*** {userSpamming.Mention} *(ID: {userSpamming.Id})*\n\nThis account has been flagged by our Raid Control Systems,\nan indefinite mute has been applied while your conduct's investigated.")
                        .WithCurrentTimestamp()
                        .WithFooter(footer =>
                        {
                            footer
                            .WithText($"Muted by RCS");
                        });

                    Embed embed = embedBuilder.Build();
                    await socketMessage.Channel.SendMessageAsync(embed: embed);

                    var embedBuilderTwo = new EmbedBuilder()
                        .WithTitle("Spam Detection Flag")
                        .WithColor(Color.DarkRed)
                        .WithDescription($"***Presumed Suspect:*** {userSpamming.Mention} *(ID: {userSpamming.Id})*\n***Time Span:*** {timeResult} second(s)\n***Content Sent:***\n {string.Join(",\n\n", messages)}")
                        .WithCurrentTimestamp();

                    Embed embedTwo = embedBuilderTwo.Build();
                    await rcsLogChannel.SendMessageAsync(embed: embedTwo);

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("ALERT: Spam Detected");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"Suspected User: {socketMessage.Author} Spam Time: {timeResult}");

                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            } catch (Exception){
                //ignore
            }*/
            
        }

        public async Task AnnounceJoinedUser(SocketGuildUser userThatJoined){
            // parses Emote so bot can use
            Emote welcomeEmote = Emote.Parse("<:bouncerkitty:1020394446988247110>");
            // gets server and channels to send message in
            var bugsServer = _client.GetGuild(960957925143224340);
            var welcomeChannel = _client.GetChannel(960957925143224343) as SocketTextChannel;
            // welcomes the new user
            var serverName = bugsServer.Name;
            var serverIconUrl = bugsServer.IconUrl;
            var rolesChannel = bugsServer.GetChannel(967910064457388142);
            var rulesChannel = bugsServer.GetChannel(965666858466426920);
            var partyEmoji = new Emoji("🎉");
            var memberCount = bugsServer.MemberCount;
            // grabs the member role
            var memberRole = userThatJoined.Guild.Roles.FirstOrDefault(x => x.Id == 965730861037264936);

            await userThatJoined.AddRoleAsync(memberRole);

            var embedBuilder = new EmbedBuilder()
                .WithColor(Color.DarkPurple)
                .WithAuthor($"{userThatJoined}", userThatJoined.GetAvatarUrl())
                .WithThumbnailUrl(serverIconUrl)
                .WithTitle($"Welcome to {serverName} !")
                .WithDescription($"- get your roles in <#{rolesChannel.Id}>\n- please read the rules <#{rulesChannel.Id}>\n")
                .WithFooter($"{partyEmoji} Membership Count: {memberCount} {partyEmoji}")
                .WithCurrentTimestamp();


            Embed embed = embedBuilder.Build();
            await welcomeChannel.SendMessageAsync(embed: embed);
        }

        public async Task JoinLogging(SocketGuildUser userJoined){
            var loggingChannel = _client.GetChannel(965699250522558566) as SocketTextChannel;
            IEmote joinEmote = Emote.Parse("<a:join:993953953832251542>");
            await loggingChannel.SendMessageAsync($"{joinEmote} {userJoined.Mention} has appeared.");
        }

        public async Task LeaveLogging(SocketGuild thisGuild, SocketUser userLeave){
            var loggingChannel = _client.GetChannel(965699250522558566) as SocketTextChannel;
            IEmote leaveEmote = Emote.Parse("<a:leave:993953885339267173>");
            await loggingChannel.SendMessageAsync($"{leaveEmote} **{userLeave.Username}** has vanished.");
        }
    }
}
