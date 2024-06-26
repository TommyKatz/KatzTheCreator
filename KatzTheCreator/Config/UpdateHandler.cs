using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.Reactive.Linq;
//using System.Data.SQLite;

namespace KatzTheCreator.Config{
    public class UpdateHandler{
        
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        //private readonly SQLiteConnection _dbConnection;

        public UpdateHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services){ // SQLiteConnection dbConnection


            _commands = commands;
            _client = client;
            _services = services;
            //_dbConnection = dbConnection;

            _client.MessageReceived += MessageOpsAsync;
            _client.ButtonExecuted += RegisterButtonHandler;
            _client.UserBanned += UserBannedLog;
            _client.UserUnbanned += UserUnbannedLog;
            _client.UserJoined += JoinedUserOps;
            _client.UserLeft += LeaveLogging;
            _client.GuildMemberUpdated += UserUpdatedOps;
            _client.UserVoiceStateUpdated += UserVoiceStateChanged;
            _client.MessageDeleted += MessageDeletedLog;
            _client.MessageUpdated += MessageEditedLog;
            _client.MessagesBulkDeleted += MessageBulkDeleteLog;
            _client.InviteCreated += InviteTracker;
        }

        private Dictionary<string, ulong> claimedTickets = new Dictionary<string, ulong>();

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
                            .WithAuthor($"{timeOutAuthor.Username} (ID: {timeOutAuthor.Id})", timeOutAuthor.GetAvatarUrl())
                            .WithDescription($"**Timed Out:** {userStateAfter.Mention} *(ID: {userStateAfter.Id})*\n**Reason:** {timoutReason}")
                            .WithCurrentTimestamp();
                        Embed embedTwo = builder.Build();
                        await loggingChannel.SendMessageAsync(embed: embedTwo);
                    } catch (Exception){
                        await loggingChannel.SendMessageAsync($"{timeOutAuthor.Mention}, an exception has been caught, please contact suicidekatz.");
                    }
                } else{
                    try{
                        var builder = new EmbedBuilder()
                            .WithColor(Color.Gold)
                            .WithThumbnailUrl(userStateAfter.GetAvatarUrl())
                            .WithAuthor($"{timeOutAuthor.Username} (ID: {timeOutAuthor.Id})", timeOutAuthor.GetAvatarUrl())
                            .WithDescription($"**Timed Out:** {userStateAfter.Mention} *(ID: {userStateAfter.Id})*\n**Reason:** No Reason Provided.")
                            .WithCurrentTimestamp();
                        Embed embedTwo = builder.Build();
                        await loggingChannel.SendMessageAsync(embed: embedTwo);
                    }catch (Exception){
                        await loggingChannel.SendMessageAsync($"{timeOutAuthor.Mention}, an exception has been caught, please contact suicidekatz.");
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
                        .WithAuthor($"{timeOutAuthor.Username} (ID: {timeOutAuthor.Id})", timeOutAuthor.GetAvatarUrl())
                        .WithDescription($"**Timed Out Removed:** {userStateAfter.Mention} *(ID: {userStateAfter.Id})*\n**Reason:** reason input unavailable.")
                        .WithCurrentTimestamp();
                    Embed embedTwo = builder.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);
                }catch (Exception){
                    await loggingChannel.SendMessageAsync($"{timeOutAuthor.Mention}, an exception has been caught, please contact suicidekatz.");
                }
            }

            if (!userStateBefore.Roles.Contains(boostRole) && userStateAfter.Roles.Contains(boostRole)){
                var boostAnnounceChannel = _client.GetChannel(960957925143224343) as SocketTextChannel;
                var boostCount = userStateAfter.Guild.PremiumSubscriptionCount;
                var server = _client.GetGuild(960957925143224340);

                var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.Purple)
                    .WithAuthor($"{userStateAfter.Username}", userStateAfter.GetAvatarUrl())
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
                            .WithAuthor($"{banAuthor.Username} (ID: {banAuthor.Id})", banAuthor.GetAvatarUrl())
                            .WithDescription($"**Banned:** {userBanned.Mention} *(ID: {userBanned.Id})*\n**Reason:** {banReason}")
                            .WithCurrentTimestamp()
                            .WithFooter(footer =>{
                            footer
                            .WithText("Right Click");
                            });
                        Embed embedTwo = builderTwo.Build();
                        await loggingChannel.SendMessageAsync(embed: embedTwo);
                    } catch (Exception){
                        await loggingChannel.SendMessageAsync($"{banAuthor.Mention}, an exception has been caught, please contact suicidekatz.");
                    }

                }else{
                    try{
                        var builderTwo = new EmbedBuilder()
                            .WithColor(Color.DarkRed)
                            .WithThumbnailUrl(userBanned.GetAvatarUrl())
                            .WithAuthor($"{banAuthor.Username} (ID: {banAuthor.Id})", banAuthor.GetAvatarUrl())
                            .WithDescription($"**Banned:** {userBanned.Mention} *(ID: {userBanned.Id})*\n**Reason:** No Reason Provided")
                            .WithCurrentTimestamp()
                            .WithFooter(footer =>{
                            footer
                            .WithText("Right Click");
                            });
                        Embed embedTwo = builderTwo.Build();
                        await loggingChannel.SendMessageAsync(embed: embedTwo);
                    } catch (Exception){
                        await loggingChannel.SendMessageAsync($"{banAuthor.Mention}, an exception has been caught, please contact suicidekatz.");
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
                    .WithAuthor($"{unbanAuthor.Username} (ID: {unbanAuthor.Id})", unbanAuthor.GetAvatarUrl())
                    .WithDescription($"**Unbanned:** {userUnbanned.Mention} *(ID: {userUnbanned.Id})*\n**Reason:** No Reason Provided")
                    .WithCurrentTimestamp()
                    .WithFooter(footer =>{
                        footer
                        .WithText("Right Click");
                    });
                    Embed embedTwo = builderTwo.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);
                } catch{
                    await loggingChannel.SendMessageAsync($"{unbanAuthor.Mention}, an exception has been caught, please contact suicidekatz.");
                }
            }
        }

        public async Task RegisterButtonHandler(SocketMessageComponent component){

            var rUser = component.User as SocketGuildUser;
            ulong scgRoleID = 965700697679077406;
            ulong killerRoleID = 965702660542062653;
            ulong scientistRoleID = 1232216233483436045;
            ulong rogueRoleID = 1232216391143260210;
            ulong usecRoleID = 1117540158112268308;
            ulong bearRoleID = 1117540205856038962;
            var getSCGRole = rUser.Guild.Roles.FirstOrDefault(x => x.Id == 965700697679077406);
            var getKillerRole = rUser.Guild.Roles.FirstOrDefault(x => x.Id == 965702660542062653);
            var getScientistRole = rUser.Guild.Roles.FirstOrDefault(x => x.Id == 1232216233483436045);
            var getRogueRole = rUser.Guild.Roles.FirstOrDefault(x => x.Id == 1232216391143260210);
            var getUsecRole = rUser.Guild.Roles.FirstOrDefault(x => x.Id == 1117540158112268308);
            var getBearRole = rUser.Guild.Roles.FirstOrDefault(x => x.Id == 1117540205856038962);

            var ticketId = component.Message.Id.ToString(); // Use the message ID as the ticket identifier
            claimedTickets[ticketId] = component.User.Id;

            switch (component.Data.CustomId){

                case "Skill Check Gang":

                    if (!rUser.Roles.Contains(getSCGRole)){

                        await rUser.AddRoleAsync(scgRoleID);
                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{component.User.Mention}, i assigned you the {getSCGRole.Mention} role.");
                        Embed embed = embedBuilder.Build();
                        await component.RespondAsync(embed: embed, ephemeral: true);

                    }else{

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

                    }else{

                        await rUser.RemoveRoleAsync(killerRoleID);
                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{component.User.Mention}, i removed your {getKillerRole.Mention} role.");
                        Embed embed = embedBuilder.Build();
                        await component.RespondAsync(embed: embed, ephemeral: true);
                    }
                    break;

                case "USEC":

                    if (!rUser.Roles.Contains(getBearRole)){

                        if (!rUser.Roles.Contains(getUsecRole)){

                            await rUser.AddRoleAsync(usecRoleID);
                            var embedBuilder = new EmbedBuilder()
                                .WithColor(Color.DarkPurple)
                                .WithDescription($"{component.User.Mention}, i assigned you the {getUsecRole.Mention} role.");
                            Embed embed = embedBuilder.Build();
                            await component.RespondAsync(embed: embed, ephemeral: true);

                        }else{

                            await rUser.RemoveRoleAsync(usecRoleID);
                            var embedBuilder = new EmbedBuilder()
                                .WithColor(Color.DarkPurple)
                                .WithDescription($"{component.User.Mention}, i removed your {getUsecRole.Mention} role.");
                            Embed embed = embedBuilder.Build();
                            await component.RespondAsync(embed: embed, ephemeral: true);
                        }

                    }else{

                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{component.User.Mention}, you may only have one of the roles at a time.");
                        Embed embed = embedBuilder.Build();
                        await component.RespondAsync(embed: embed, ephemeral: true);

                    }
                    break;

                case "BEAR":

                    if (!rUser.Roles.Contains(getUsecRole)){

                        if (!rUser.Roles.Contains(getBearRole)){

                            await rUser.AddRoleAsync(bearRoleID);
                            var embedBuilder = new EmbedBuilder()
                                .WithColor(Color.DarkPurple)
                                .WithDescription($"{component.User.Mention}, i assigned you the {getBearRole.Mention} role.");
                            Embed embed = embedBuilder.Build();
                            await component.RespondAsync(embed: embed, ephemeral: true);

                        }else{

                            await rUser.RemoveRoleAsync(bearRoleID);
                            var embedBuilder = new EmbedBuilder()
                                .WithColor(Color.DarkPurple)
                                .WithDescription($"{component.User.Mention}, i removed your {getBearRole.Mention} role.");
                            Embed embed = embedBuilder.Build();
                            await component.RespondAsync(embed: embed, ephemeral: true);

                        }

                    }else{

                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{component.User.Mention}, you may only have one of the roles at a time.");
                        Embed embed = embedBuilder.Build();
                        await component.RespondAsync(embed: embed, ephemeral: true);

                    }

                    break;

                case "Science Team":

                    if (!rUser.Roles.Contains(getRogueRole)){

                        if (!rUser.Roles.Contains(getScientistRole)){

                            await rUser.AddRoleAsync(scientistRoleID);
                            var embedBuilder = new EmbedBuilder()
                                .WithColor(Color.DarkPurple)
                                .WithDescription($"{component.User.Mention}, i assigned you the {getScientistRole.Mention} role.");
                            Embed embed = embedBuilder.Build();
                            await component.RespondAsync(embed: embed, ephemeral: true);

                        }else{

                            await rUser.RemoveRoleAsync(scientistRoleID);
                            var embedBuilder = new EmbedBuilder()
                                .WithColor(Color.DarkPurple)
                                .WithDescription($"{component.User.Mention}, i removed your {getScientistRole.Mention} role.");
                            Embed embed = embedBuilder.Build();
                            await component.RespondAsync(embed: embed, ephemeral: true);
                        }

                    }else{

                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{component.User.Mention}, you may only have one of the roles at a time.");
                        Embed embed = embedBuilder.Build();
                        await component.RespondAsync(embed: embed, ephemeral: true);

                    }
                    break;

                case "Rogue Scientist":

                    if (!rUser.Roles.Contains(getScientistRole)){

                        if (!rUser.Roles.Contains(getRogueRole)){

                            await rUser.AddRoleAsync(rogueRoleID);
                            var embedBuilder = new EmbedBuilder()
                                .WithColor(Color.DarkPurple)
                                .WithDescription($"{component.User.Mention}, i assigned you the {getRogueRole.Mention} role.");
                            Embed embed = embedBuilder.Build();
                            await component.RespondAsync(embed: embed, ephemeral: true);

                        }else{

                            await rUser.RemoveRoleAsync(rogueRoleID);
                            var embedBuilder = new EmbedBuilder()
                                .WithColor(Color.DarkPurple)
                                .WithDescription($"{component.User.Mention}, i removed your {getRogueRole.Mention} role.");
                            Embed embed = embedBuilder.Build();
                            await component.RespondAsync(embed: embed, ephemeral: true);

                        }

                    }else{

                        var embedBuilder = new EmbedBuilder()
                            .WithColor(Color.DarkPurple)
                            .WithDescription($"{component.User.Mention}, you may only have one of the roles at a time.");
                        Embed embed = embedBuilder.Build();
                        await component.RespondAsync(embed: embed, ephemeral: true);

                    }

                    break;

                case "Claim":
                    Emoji checkEmoji = new Emoji("✔️");
                    Emoji trashEmoji = new Emoji("🗑️");
                    Emoji falseEmoji = new Emoji("✖️");
                    var userToClaim = component.User as SocketGuildUser;
                    if (userToClaim.Roles.All(r => r.Equals(Discord.Color.Default))) return;

                    var filterOutDefault = userToClaim.Roles.Where(r => r.Color != Discord.Color.Default);
                    var userHighestRoleColor = filterOutDefault.MaxBy(r => r.Position).Color;

                    await component.UpdateAsync(o =>{

                        o.Embed = component.Message.Embeds.First().ToEmbedBuilder()
                        .WithColor(userHighestRoleColor)
                        .WithCurrentTimestamp()
                        .WithFooter(footer =>{

                            footer
                            .WithText($"Claimed by {component.User.Username}:")
                            .WithIconUrl(component.User.GetAvatarUrl());
                        })
                        .Build();
                        o.Components = new ComponentBuilder()
                            .WithButton("Completed", "Completed", ButtonStyle.Success, checkEmoji)
                            .WithButton("Trash", "Trash", ButtonStyle.Secondary, trashEmoji)
                            .WithButton("False Report", "False Report", ButtonStyle.Danger, falseEmoji).Build();

                    });



                    break;

                case "Completed":{
                        var ticketIdCompleted = component.Message.Id.ToString();
                        if (!claimedTickets.TryGetValue(ticketIdCompleted, out var claimedUserId) || claimedUserId != component.User.Id){
                            // The user who clicked this button did not claim the ticket
                            await component.DeferAsync();
                            await component.FollowupAsync($"You did not claim this ticket, so you cannot interact with these buttons.", ephemeral: true);
                            return;
                        }

                        var loggingChannel = rUser.Guild.GetTextChannel(1126979920376119346);
                        var embedCopy = component.Message.Embeds.First().ToEmbedBuilder()
                            .WithColor(Color.Green)
                            .WithThumbnailUrl("https://i.imgur.com/YpmReEl.png")
                            .WithCurrentTimestamp()
                            .WithFooter(footer =>{
                                footer
                                .WithText($"Completed by {component.User.Username}:")
                                .WithIconUrl(component.User.GetAvatarUrl());
                            })
                            .Build();

                        await component.DeferAsync();
                        await component.DeleteOriginalResponseAsync();

                        await loggingChannel.SendMessageAsync(embed: embedCopy);

                        break;
                    }

                case "Trash":{
                        var ticketIdTrash = component.Message.Id.ToString();
                        if (!claimedTickets.TryGetValue(ticketIdTrash, out var claimedUserId) || claimedUserId != component.User.Id){
                            // The user who clicked this button did not claim the ticket
                            await component.DeferAsync();
                            await component.FollowupAsync($"You did not claim this ticket, so you cannot interact with these buttons.", ephemeral: true);
                            return;
                        }

                        var loggingTwoChannel = rUser.Guild.GetTextChannel(1126979920376119346);
                        var embedTwoCopy = component.Message.Embeds.First().ToEmbedBuilder()
                            .WithColor(Color.LighterGrey)
                            .WithThumbnailUrl("https://i.imgur.com/MyOqEvL.png")
                            .WithCurrentTimestamp()
                            .WithFooter(footer =>{
                                footer
                                .WithText($"Trashed by {component.User.Username}:")
                                .WithIconUrl(component.User.GetAvatarUrl());
                            })
                            .Build();

                        await component.DeferAsync();
                        await component.DeleteOriginalResponseAsync();

                        await loggingTwoChannel.SendMessageAsync(embed: embedTwoCopy);

                        break;
                    }

                case "False Report":{
                        var ticketIdFalseReport = component.Message.Id.ToString();
                        if (!claimedTickets.TryGetValue(ticketIdFalseReport, out var claimedUserId) || claimedUserId != component.User.Id){
                            // The user who clicked this button did not claim the ticket
                            await component.DeferAsync();
                            await component.FollowupAsync($"You did not claim this ticket, so you cannot interact with these buttons.", ephemeral: true);
                            return;
                        }

                        var loggingThreeChannel = rUser.Guild.GetTextChannel(1126979920376119346);
                        var embedThreeCopy = component.Message.Embeds.First().ToEmbedBuilder()
                            .WithColor(Color.DarkRed)
                            .WithThumbnailUrl("https://i.imgur.com/8nj70eE.png")
                            .WithCurrentTimestamp()
                            .WithFooter(footer =>{
                                footer
                                .WithText($"Falsified by {component.User.Username}:")
                                .WithIconUrl(component.User.GetAvatarUrl());
                            })
                            .Build();

                        await component.DeferAsync();
                        await component.DeleteOriginalResponseAsync();

                        await loggingThreeChannel.SendMessageAsync(embed: embedThreeCopy);

                        break;
                    }

            }
        }
            /*public void StoreInfo(string username, ulong userId, ulong msgsTyped, ulong userLevel){

                using (var cmd = new SQLiteCommand("INSERT INTO MessageUsers", _dbConnection)){
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@UserId", userId.ToString());
                    cmd.Parameters.AddWithValue("@UserMessagesTyped", msgsTyped.ToString());
                    cmd.Parameters.AddWithValue("@UserLevel", userLevel.ToString());


                    cmd.ExecuteNonQuery();
                }
            }*/

            public async Task MessageOpsAsync(SocketMessage msg){
            if (string.IsNullOrEmpty(msg.Content) && msg.Attachments.Count == 0) return;
            if (msg.Author.IsBot) return;
            if (msg.Channel.Id == 1046883404123222096) await msg.DeleteAsync();

            if (msg.Channel.Id == 1051719930384490536 && msg.Attachments.Count >= 1){
                try{
                    Emote heartyEmote = Emote.Parse("<:hearty:1100477170771566604>");
                    await msg.AddReactionAsync(heartyEmote);
                }
                catch (Exception){
                    //ignore
                }
            } 

            Random rnd = new Random();
            int number = rnd.Next(1, 101);

            if (number <= 4 && msg.Channel.GetChannelType() != ChannelType.DM){ // 4% chance

                try{
                    Emote hmmEmote = Emote.Parse("<:hmm:1025075030540959775>");
                    Emote ummEmote = Emote.Parse("<:umm:1038944243655397558>");
                    Emote youGoodEmote = Emote.Parse("<:yougood:1038944411196866681>");
                    Emote lowIqEmote = Emote.Parse("<:lowiq:1038944596253745273>");
                    Emote whyYouLikeThisEmote = Emote.Parse("<:whyyoulikethis:1038944821953429584>");

                    Random rndTwo = new Random();
                    int numberTwo = rndTwo.Next(1, 6);

                    if (numberTwo == 1){
                        await msg.AddReactionAsync(hmmEmote);
                    }else if (numberTwo == 2){
                        await msg.AddReactionAsync(ummEmote);
                    }else if (numberTwo == 3){
                        await msg.AddReactionAsync(youGoodEmote);
                    }else if (numberTwo == 4){
                        await msg.AddReactionAsync(lowIqEmote);
                    }else{
                        await msg.AddReactionAsync(whyYouLikeThisEmote);
                    }
                }catch (Exception){
                    // ignore
                }
            }
        }

        public async Task MessageDeletedLog(Cacheable<IMessage, ulong> msg, Cacheable<IMessageChannel, ulong> channel){
            try{
                var loggingChannel = _client.GetChannel(965723854955773952) as SocketTextChannel;
                var bot = _client.CurrentUser;

                List<ulong> channelId = new List<ulong>();
                channelId.Add(960957925143224343); // gen
                channelId.Add(1009837099085742110); // announcemnets
                channelId.Add(965666858466426920); // rules
                channelId.Add(1020452407324459069); // guide
                IEnumerable<ulong> allowedIds = channelId;

                List<ulong> modChannelId = new List<ulong>();
                modChannelId.Add(965699174358216744); // mod
                modChannelId.Add(965723854955773952); // deleted
                modChannelId.Add(1122241848254148620); // invite
                modChannelId.Add(965699096352526366); // voice
                IEnumerable<ulong> moderatedIds = channelId;

                if (msg.HasValue && msg.Value.Embeds.Count > 0 && modChannelId.Contains(channel.Id)){

                    var deletedMessage = msg.Value;
                    var onlyEmbed = deletedMessage.Embeds.FirstOrDefault();
                    var appInfo = await _client.GetApplicationInfoAsync();
                    var owner = appInfo.Owner;
                    var emergChannel = _client.GetChannel(988184539455172688) as SocketTextChannel;

                    if (deletedMessage is IUserMessage userMessage && userMessage.Channel is IGuildChannel guildChannel){

                        var guild = guildChannel.Guild;
                        var auditLogs = await guild.GetAuditLogsAsync();

                        var messageDeleteLogUser = (await guild.GetAuditLogsAsync(actionType: ActionType.MessageDeleted)).FirstOrDefault().User;

                        if (onlyEmbed != null && messageDeleteLogUser != null){

                            string embedDetails = $"Author: {onlyEmbed.Author?.Name}\n";

                            foreach (var field in onlyEmbed.Fields){
                                embedDetails += $"{field.Name}: {field.Value}\n";  

                            }

                            try{
                                await owner.SendMessageAsync($"{messageDeleteLogUser.Mention} (ID: {messageDeleteLogUser.Id}) deleted the following embed in <#{channel.Id}> and it now flagged as a **high risk** moderator:\n\n{embedDetails}");
                            } catch {
                                await emergChannel.SendMessageAsync($"{messageDeleteLogUser.Mention} (ID: {messageDeleteLogUser.Id}) deleted the following embed in <#{channel.Id}> and it now flagged as a **high risk** moderator:\n\n{embedDetails}");
                            }
                            

                        }


                    }


                    
                }

                if (!msg.HasValue || !allowedIds.Contains(channel.Id)) return;

                if (msg.Value.Author.GetAvatarUrl() != null){

                    var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.DarkMagenta)
                    .WithAuthor($"{msg.Value.Author.Username}", msg.Value.Author.GetAvatarUrl())
                    .WithDescription($"Message deleted in <#{channel.Id}>")
                    .AddField("Content", $"{msg.Value.Content}")
                    .AddField("Date", $"<t:{msg.Value.Timestamp.ToUnixTimeSeconds()}:F>")
                    .AddField("ID", $"```ini\nUser = {msg.Value.Author.Id}\nMessage = {msg.Value.Id}\nChannel = {channel.Id}\n```")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithIconUrl(bot.GetAvatarUrl())
                        .WithText($"{bot}");
                    });

                    var embed = embedBuilder.Build();

                    await loggingChannel.SendMessageAsync(embed: embed);

                }
                else{

                    var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.DarkMagenta)
                    .WithAuthor($"{msg.Value.Author.Username}", msg.Value.Author.GetDefaultAvatarUrl())
                    .WithDescription($"Message deleted in <#{channel.Id}>")
                    .AddField("Content", $"{msg.Value.Content}")
                    .AddField("Date", $"<t:{msg.Value.Timestamp.ToUnixTimeSeconds()}:F>")
                    .AddField("ID", $"```ini\nUser = {msg.Value.Author.Id}\nMessage = {msg.Value.Id}\nChannel = {channel.Id}\n```")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithIconUrl(bot.GetAvatarUrl())
                        .WithText($"{bot}");
                    });

                    var embed = embedBuilder.Build();

                    await loggingChannel.SendMessageAsync(embed: embed);

                }
            }
            catch (Exception){
                //ignore
            }

        }

        public async Task MessageEditedLog(Cacheable<IMessage, ulong> msgBefore, SocketMessage msgAfter, ISocketMessageChannel channel){
            try{
                var loggingChannel = _client.GetChannel(965723854955773952) as SocketTextChannel;
                var bot = _client.CurrentUser;

                List<ulong> channelId = new List<ulong>();
                channelId.Add(960957925143224343); // gen
                channelId.Add(1009837099085742110); // announcemnets
                channelId.Add(965666858466426920); // rules
                channelId.Add(1020452407324459069); // guide
                IEnumerable<ulong> allowedIds = channelId;

                if (!allowedIds.Contains(channel.Id)) return;
                if (msgAfter.Embeds.Any()) return;

                if (msgAfter.Author.GetAvatarUrl() != null){

                    var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.DarkMagenta)
                    .WithAuthor($"{msgAfter.Author.Username}", msgAfter.Author.GetAvatarUrl())
                    .WithDescription($"Message edited in <#{channel.Id}>")
                    .AddField("Now", $"{msgAfter}")
                    .AddField("Previous", $"{msgBefore.Value.Content}")
                    .AddField("ID", $"```ini\nUser = {msgAfter.Author.Id}\nMessage = {msgAfter.Id}\nChannel = {channel.Id}\n```")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithIconUrl(bot.GetAvatarUrl())
                        .WithText($"{bot}");
                    });

                    var embed = embedBuilder.Build();

                    await loggingChannel.SendMessageAsync(embed: embed);

                }
                else{

                    var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.DarkMagenta)
                    .WithAuthor($"{msgAfter.Author.Username}", msgAfter.Author.GetDefaultAvatarUrl())
                    .WithDescription($"Message edited in <#{channel.Id}>")
                    .AddField("Now", $"{msgAfter}")
                    .AddField("Previous", $"{msgBefore.Value.Content}")
                    .AddField("ID", $"```ini\nUser = {msgAfter.Author.Id}\nMessage = {msgAfter.Id}\nChannel = {channel.Id}\n```")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithIconUrl(bot.GetAvatarUrl())
                        .WithText($"{bot}");
                    });

                    var embed = embedBuilder.Build();

                    await loggingChannel.SendMessageAsync(embed: embed);

                }

            }catch (Exception){ 
                // ignore
            }
            
        }

        public async Task MessageBulkDeleteLog(IReadOnlyCollection<Cacheable<IMessage, ulong>> msgs, Cacheable<IMessageChannel, ulong> channel){

            try{
                var loggingChannel = _client.GetChannel(965723854955773952) as SocketTextChannel;
                var bot = _client.CurrentUser;

                List<ulong> channelId = new List<ulong>();
                channelId.Add(960957925143224343); // gen
                channelId.Add(1009837099085742110); // announcemnets
                channelId.Add(965666858466426920); // rules
                channelId.Add(1020452407324459069); // guide
                IEnumerable<ulong> allowedIds = channelId;

                if (!allowedIds.Contains(channel.Id)) return;

                var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.Magenta)
                    .WithDescription($"**{msgs.Count}** message(s) were deleted in <#{channel.Id}>")
                    .AddField("Content", $"{string.Join(", \n", msgs.Select(x => x.Value.Content))}")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithIconUrl(bot.GetAvatarUrl())
                        .WithText($"{bot}");
                    });

                var embed = embedBuilder.Build();

                await loggingChannel.SendMessageAsync(embed: embed);
            }
            catch (Exception){
                // ignore        
            }
        }

        public async Task UserVoiceStateChanged(IUser user, SocketVoiceState vStateBefore, SocketVoiceState vStateAfter){
        
            var loggingChannel = _client.GetChannel(965699096352526366) as SocketTextChannel;
            var createVcChannel = _client.GetChannel(1045071639295053854) as SocketVoiceChannel;
            var bot = _client.CurrentUser;

            if (user.GetAvatarUrl() != null){

                if (vStateBefore.VoiceChannel == null){ // joins vc

                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkBlue)
                        .WithAuthor($"{user.Username}", user.GetAvatarUrl())
                        .WithDescription($"**{user.Username}** joined voice channel: {vStateAfter.VoiceChannel}")
                        .AddField("Channel", $"{vStateAfter.VoiceChannel.Mention}")
                        .AddField("ID", $"```ini\nUser = {user.Id}\nChannel = {vStateAfter.VoiceChannel.Id}\n```")
                        .WithCurrentTimestamp()
                        .WithFooter(footer => {
                            footer
                            .WithIconUrl(bot.GetAvatarUrl())
                            .WithText($"{bot}");
                        });

                    var embed = embedBuilder.Build();

                    await loggingChannel.SendMessageAsync(embed: embed);

                    if (vStateAfter.VoiceChannel.CategoryId == 1045032443830353931 && vStateAfter.VoiceChannel != createVcChannel){
                        var rUser = user as SocketGuildUser;
                        var channelOverwrites = new OverwritePermissions(viewChannel: PermValue.Allow);
                        var cTChannel = rUser.Guild.GetTextChannel(1046883404123222096);

                        await cTChannel.AddPermissionOverwriteAsync(rUser, channelOverwrites);
                    }

                }else if (vStateAfter.VoiceChannel == null){ // disconnects from vc

                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkBlue)
                        .WithAuthor($"{user.Username}", user.GetAvatarUrl())
                        .WithDescription($"**{user.Username}** left voice channel: {vStateBefore.VoiceChannel}")
                        .AddField("Channel", $"{vStateBefore.VoiceChannel.Mention}")
                        .AddField("ID", $"```ini\nUser = {user.Id}\nChannel = {vStateBefore.VoiceChannel.Id}\n```")
                        .WithCurrentTimestamp()
                        .WithFooter(footer => {
                            footer
                            .WithIconUrl(bot.GetAvatarUrl())
                            .WithText($"{bot}");
                        });
                    var embed = embedBuilder.Build();

                    await loggingChannel.SendMessageAsync(embed: embed);

                    if (vStateBefore.VoiceChannel.CategoryId == 1045032443830353931){
                        var rUser = user as SocketGuildUser;
                        var cTChannel = rUser.Guild.GetTextChannel(1046883404123222096);
                        await cTChannel.RemovePermissionOverwriteAsync(rUser);
                    }

                }else if (vStateBefore.VoiceChannel != vStateAfter.VoiceChannel){ // moved vcs

                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkBlue)
                        .WithAuthor($"{user.Username}", user.GetAvatarUrl())
                        .WithDescription($"**{user.Username}** moved from: {vStateBefore.VoiceChannel} to {vStateAfter.VoiceChannel}")
                        .AddField("Channels", $"Before: {vStateBefore.VoiceChannel.Mention}\nAfter: {vStateAfter.VoiceChannel.Mention}")
                        .AddField("ID", $"```ini\nUser = {user.Id}\nNew = {vStateAfter.VoiceChannel.Id}\nOld = {vStateBefore.VoiceChannel.Id}\n```")
                        .WithCurrentTimestamp()
                        .WithFooter(footer => {
                            footer
                            .WithIconUrl(bot.GetAvatarUrl())
                            .WithText($"{bot}");
                        });
                    var embed = embedBuilder.Build();

                    await loggingChannel.SendMessageAsync(embed: embed);

                    /*if (vStateAfter.VoiceChannel.CategoryId == 1045032443830353931)
                    {
                        var rUser = user as SocketGuildUser;
                        var channelOverwrites = new OverwritePermissions(viewChannel: PermValue.Allow);
                        var cTChannel = rUser.Guild.GetTextChannel(1046883404123222096);

                        await cTChannel.AddPermissionOverwriteAsync(rUser, channelOverwrites);
                    } else if (vStateAfter.VoiceChannel.CategoryId != 1045032443830353931 && vStateAfter.VoiceChannel != createVcChannel){
                        var rUser = user as SocketGuildUser;
                        var cTChannel = rUser.Guild.GetTextChannel(1046883404123222096);
                        await cTChannel.RemovePermissionOverwriteAsync(rUser);
                    }*/
                }

            } else {

                if (vStateBefore.VoiceChannel == null){ // joins vc

                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkBlue)
                        .WithAuthor($"{user.Username}", user.GetDefaultAvatarUrl())
                        .WithDescription($"**{user.Username}** joined voice channel: {vStateAfter.VoiceChannel}")
                        .AddField("Channel", $"{vStateAfter.VoiceChannel.Mention}")
                        .AddField("ID", $"```ini\nUser = {user.Id}\nChannel = {vStateAfter.VoiceChannel.Id}\n```")
                        .WithCurrentTimestamp()
                        .WithFooter(footer => {
                            footer
                            .WithIconUrl(bot.GetAvatarUrl())
                            .WithText($"{bot}");
                        });

                    var embed = embedBuilder.Build();

                    await loggingChannel.SendMessageAsync(embed: embed);

                    if (vStateAfter.VoiceChannel.CategoryId == 1045032443830353931 && vStateAfter.VoiceChannel != createVcChannel){
                        var rUser = user as SocketGuildUser;
                        var channelOverwrites = new OverwritePermissions(viewChannel: PermValue.Allow);
                        var cTChannel = rUser.Guild.GetTextChannel(1046883404123222096);

                        await cTChannel.AddPermissionOverwriteAsync(rUser, channelOverwrites);
                    }

                }
                else if (vStateAfter.VoiceChannel == null){ // disconnects from vc

                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkBlue)
                        .WithAuthor($"{user.Username}", user.GetDefaultAvatarUrl())
                        .WithDescription($"**{user.Username}** left voice channel: {vStateBefore.VoiceChannel}")
                        .AddField("Channel", $"{vStateBefore.VoiceChannel.Mention}")
                        .AddField("ID", $"```ini\nUser = {user.Id}\nChannel = {vStateBefore.VoiceChannel.Id}\n```")
                        .WithCurrentTimestamp()
                        .WithFooter(footer => {
                            footer
                            .WithIconUrl(bot.GetAvatarUrl())
                            .WithText($"{bot}");
                        });
                    var embed = embedBuilder.Build();

                    await loggingChannel.SendMessageAsync(embed: embed);

                    if (vStateBefore.VoiceChannel.CategoryId == 1045032443830353931){
                        var rUser = user as SocketGuildUser;
                        var cTChannel = rUser.Guild.GetTextChannel(1046883404123222096);
                        await cTChannel.RemovePermissionOverwriteAsync(rUser);
                    }

                }
                else if (vStateBefore.VoiceChannel != vStateAfter.VoiceChannel){ // moved vcs

                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkBlue)
                        .WithAuthor($"{user.Username}", user.GetDefaultAvatarUrl())
                        .WithDescription($"**{user.Username}** moved from: {vStateBefore.VoiceChannel} to {vStateAfter.VoiceChannel}")
                        .AddField("Channels", $"Before: {vStateBefore.VoiceChannel.Mention}\nAfter: {vStateAfter.VoiceChannel.Mention}")
                        .AddField("ID", $"```ini\nUser = {user.Id}\nNew = {vStateAfter.VoiceChannel.Id}\nOld = {vStateBefore.VoiceChannel.Id}\n```")
                        .WithCurrentTimestamp()
                        .WithFooter(footer => {
                            footer
                            .WithIconUrl(bot.GetAvatarUrl())
                            .WithText($"{bot}");
                        });
                    var embed = embedBuilder.Build();

                    await loggingChannel.SendMessageAsync(embed: embed);

                    /*if (vStateAfter.VoiceChannel.CategoryId == 1045032443830353931)
                    {
                        var rUser = user as SocketGuildUser;
                        var channelOverwrites = new OverwritePermissions(viewChannel: PermValue.Allow);
                        var cTChannel = rUser.Guild.GetTextChannel(1046883404123222096);

                        await cTChannel.AddPermissionOverwriteAsync(rUser, channelOverwrites);
                    } else if (vStateAfter.VoiceChannel.CategoryId != 1045032443830353931 && vStateAfter.VoiceChannel != createVcChannel){
                        var rUser = user as SocketGuildUser;
                        var cTChannel = rUser.Guild.GetTextChannel(1046883404123222096);
                        await cTChannel.RemovePermissionOverwriteAsync(rUser);
                    }*/
                }

            }

            

            /*if (vStateAfter.VoiceChannel == createVcChannel){
                var rUser = user as SocketGuildUser;
                var thisGuild = rUser.Guild as IGuild;
                try{
                    var auditCheckVCCreated = (await thisGuild.GetAuditLogsAsync(userId: 1022688631690891345, actionType: ActionType.ChannelCreated)).FirstOrDefault().CreatedAt.DateTime;
                    var timeResult = (DateTime.UtcNow - auditCheckVCCreated).TotalSeconds;
                    if (timeResult >= 6){
                        var categoryId = createVcChannel.CategoryId;

                        // viewChannel = Owner | connect = Authed | speak = Mod
                        var channelOverwrites = new OverwritePermissions(viewChannel: PermValue.Allow, connect: PermValue.Allow);

                        var newVC = await rUser.Guild.CreateVoiceChannelAsync($"{user.Username}'s VC", tcp => tcp.CategoryId = categoryId);

                        await newVC.AddPermissionOverwriteAsync(rUser, channelOverwrites);

                        await rUser.ModifyAsync(x => { x.Channel = newVC; });
                    }
                    else
                    {
                        await rUser.ModifyAsync(x => { x.Channel = null; });
                    }
                }catch (Exception noAuditLogging){
                    var categoryId = createVcChannel.CategoryId;

                    var channelOverwrites = new OverwritePermissions(viewChannel: PermValue.Allow, connect: PermValue.Allow);

                    var newVC = await rUser.Guild.CreateVoiceChannelAsync($"{user.Username}'s VC", tcp => tcp.CategoryId = categoryId);

                    await newVC.AddPermissionOverwriteAsync(rUser, channelOverwrites);

                    await rUser.ModifyAsync(x => { x.Channel = newVC; });
                }
            }

            if (vStateBefore.VoiceChannel != null){
                if (vStateBefore.VoiceChannel.CategoryId == 1045032443830353931 && vStateBefore.VoiceChannel.ConnectedUsers.Count == 0 && vStateBefore.VoiceChannel != createVcChannel){
                    await vStateBefore.VoiceChannel.DeleteAsync();
                }
            }*/
            
        }

        public async Task InviteTracker(SocketInvite guildInvite){
            var trackingChannel = _client.GetChannel(1122241848254148620) as SocketTextChannel;
            var inviter = guildInvite.Inviter;
            var bot = _client.CurrentUser;

            if (inviter.GetAvatarUrl() != null){

                if (guildInvite.ExpiresAt != null){
                    var embedBuilder = new EmbedBuilder()
                   .WithColor(Color.DarkPurple)
                   .WithAuthor($"{inviter.Username}", inviter.GetAvatarUrl())
                   .WithTitle("Invite Created")
                   .WithDescription($"**Inviter:** *<@{inviter.Id}> - {inviter.Id}*")
                   .AddField("Information", $"**{guildInvite.Url}**\n```ini\nExpiration = {guildInvite.ExpiresAt.Value.DateTime}\nChannel = {guildInvite.ChannelId}\n```")
                   .WithCurrentTimestamp()
                   .WithFooter(footer => {
                       footer
                       .WithIconUrl(bot.GetAvatarUrl())
                       .WithText($"{bot}");
                   });

                    Embed embed = embedBuilder.Build();
                    await trackingChannel.SendMessageAsync(embed: embed);
                }else{
                    var embedBuilder = new EmbedBuilder()
                   .WithColor(Color.DarkPurple)
                   .WithAuthor($"{inviter.Username}", inviter.GetAvatarUrl())
                   .WithTitle("Invite Created")
                   .WithDescription($"**Inviter:** *<@{inviter.Id}> - {inviter.Id}*")
                   .AddField("Information", $"**{guildInvite.Url}**\n```ini\nExpiration = Never\nChannel = {guildInvite.ChannelId}\n```")
                   .WithCurrentTimestamp()
                   .WithFooter(footer => {
                       footer
                       .WithIconUrl(bot.GetAvatarUrl())
                       .WithText($"{bot}");
                   });

                    Embed embed = embedBuilder.Build();
                    await trackingChannel.SendMessageAsync(embed: embed);
                }

            } else {

                if (guildInvite.ExpiresAt != null){
                    var embedBuilder = new EmbedBuilder()
                   .WithColor(Color.DarkPurple)
                   .WithAuthor($"{inviter.Username}", inviter.GetDefaultAvatarUrl())
                   .WithTitle("Invite Created")
                   .WithDescription($"**Inviter:** *<@{inviter.Id}> - {inviter.Id}*")
                   .AddField("Information", $"**{guildInvite.Url}**\n```ini\nExpiration = {guildInvite.ExpiresAt.Value.DateTime}\nChannel = {guildInvite.ChannelId}\n```")
                   .WithCurrentTimestamp()
                   .WithFooter(footer => {
                       footer
                       .WithIconUrl(bot.GetAvatarUrl())
                       .WithText($"{bot}");
                   });

                    Embed embed = embedBuilder.Build();
                    await trackingChannel.SendMessageAsync(embed: embed);
                }else{
                    var embedBuilder = new EmbedBuilder()
                   .WithColor(Color.DarkPurple)
                   .WithAuthor($"{inviter.Username}", inviter.GetDefaultAvatarUrl())
                   .WithTitle("Invite Created")
                   .WithDescription($"**Inviter:** *<@{inviter.Id}> - {inviter.Id}*")
                   .AddField("Information", $"**{guildInvite.Url}**\n```ini\nExpiration = Never\nChannel = {guildInvite.ChannelId}\n```")
                   .WithCurrentTimestamp()
                   .WithFooter(footer => {
                       footer
                       .WithIconUrl(bot.GetAvatarUrl())
                       .WithText($"{bot}");
                   });

                    Embed embed = embedBuilder.Build();
                    await trackingChannel.SendMessageAsync(embed: embed);
                }

            }

        }

        /*public static class MessageTimer{
            private static System.Timers.Timer messageTimer;
            private static SocketCommandContext _Context;

            public static void StartTimer(SocketCommandContext context){
                _Context = context;

                messageTimer = new System.Timers.Timer(86400000);
                messageTimer.Elapsed += OnTimerElapsed;
                messageTimer.AutoReset = true;
                messageTimer.Enabled = true;
            }

            public static async void OnTimerElapsed(object source, ElapsedEventArgs e){
                List<string> responses = new List<string>();
                responses.Add("Wanna hear a joke?");
                responses.Add("Hey...");
                responses.Add("I gotta good one for you...");
                responses.Add("Howdy... can i tell you a joke?");
                responses.Add("Let me tell you something silly...");
                responses.Add("<@153643067516125185> wanted me to tell y'all this...");
                responses.Add("<@135143527767080960> came up with this one...");
                IEnumerable<string> response = responses;
                Random rnd = new Random();
                var randomizeReponses = responses.OrderBy(u => rnd.Next(0, responses.Count()));
                var selectedResponse = randomizeReponses.FirstOrDefault();

                string[] allLines = File.ReadAllLines("jokes365.txt");
                Random rndTwo = new Random();
                await _Context.Channel.SendMessageAsync($"**{selectedResponse}**\n{allLines[rndTwo.Next(allLines.Length)]}");
            }

            public class StartTimedEmbed : ModuleBase<SocketCommandContext>{
                [Command("starttimer")]
                public async Task StartMessageTimer(){
                    await Context.Message.DeleteAsync();
                    MessageTimer.StartTimer(Context);
                }
            }
        }*/

        public async Task JoinedUserOps(SocketGuildUser userThatJoined){
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

            if (userThatJoined.GetAvatarUrl() != null){

                var embedBuilder = new EmbedBuilder()
                .WithColor(Color.DarkPurple)
                .WithAuthor($"{userThatJoined.DisplayName}", userThatJoined.GetAvatarUrl())
                .WithThumbnailUrl(serverIconUrl)
                .WithTitle($"Welcome to {serverName} !")
                .WithDescription($"- get your roles in <#{rolesChannel.Id}>\n- please read the rules <#{rulesChannel.Id}>\n")
                .WithFooter($"{partyEmoji} Membership Count: {memberCount} {partyEmoji}")
                .WithCurrentTimestamp();


                //announce user that joined and information in join-leave-logs
                var loggingChannel = _client.GetChannel(965699250522558566) as SocketTextChannel;
                IEmote joinEmote = Emote.Parse("<a:join:993953953832251542>");
                var bot = _client.CurrentUser;

                DateTime currentDate = DateTime.UtcNow;

                // Calculate years, months, and days since account creation
                TimeSpan accountAge = currentDate - userThatJoined.CreatedAt.UtcDateTime;
                int accountYears = (int)(accountAge.TotalDays / 365);
                int accountMonths = (int)((accountAge.TotalDays % 365) / 30);
                int accountDays = (int)(accountAge.TotalDays % 30);
                int accountHours = (int)accountAge.TotalHours;

                if (accountYears > 0)
                {

                    var embedbuilderTwo = new EmbedBuilder()
                    .WithColor(Color.DarkGreen)
                    .WithAuthor($"{userThatJoined.Username} has appeared.", userThatJoined.GetAvatarUrl())
                    .WithDescription($"{joinEmote} {userThatJoined.Mention} *(ID: {userThatJoined.Id})*")
                    .AddField("Account Created On", $"{userThatJoined.CreatedAt.UtcDateTime.ToString("D")} *({accountYears} years, {accountMonths} months, {accountDays} days)*")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithIconUrl(bot.GetAvatarUrl())
                        .WithText($"{bot}");
                    });
                    Embed embedTwo = embedbuilderTwo.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);

                }
                else if (accountMonths > 0)
                {

                    var embedbuilderTwo = new EmbedBuilder()
                    .WithColor(Color.DarkGreen)
                    .WithAuthor($"{userThatJoined.Username} has appeared.", userThatJoined.GetAvatarUrl())
                    .WithDescription($"{joinEmote} {userThatJoined.Mention} *(ID: {userThatJoined.Id})*")
                    .AddField("Account Created On", $"{userThatJoined.CreatedAt.UtcDateTime.ToString("D")} *({accountMonths} months, {accountDays} days)*")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithIconUrl(bot.GetAvatarUrl())
                        .WithText($"{bot}");
                    });
                    Embed embedTwo = embedbuilderTwo.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);

                }
                else if (accountDays > 0)
                {

                    var embedbuilderTwo = new EmbedBuilder()
                    .WithColor(Color.DarkGreen)
                    .WithAuthor($"{userThatJoined.Username} has appeared.", userThatJoined.GetAvatarUrl())
                    .WithDescription($"{joinEmote} {userThatJoined.Mention} *(ID: {userThatJoined.Id})*")
                    .AddField("Account Created On", $"{userThatJoined.CreatedAt.UtcDateTime.ToString("D")} *({accountDays} days)*")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithIconUrl(bot.GetAvatarUrl())
                        .WithText($"{bot}");
                    });
                    Embed embedTwo = embedbuilderTwo.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);

                }
                else if (accountHours > 0)
                {
                    var embedbuilderTwo = new EmbedBuilder()
                    .WithColor(Color.DarkGreen)
                    .WithAuthor($"{userThatJoined.Username} has appeared.", userThatJoined.GetAvatarUrl())
                    .WithDescription($"{joinEmote} {userThatJoined.Mention} *(ID: {userThatJoined.Id})*")
                    .AddField("Account Created On", $"{userThatJoined.CreatedAt.UtcDateTime.ToString("D")} *({accountHours} hours)*")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithIconUrl(bot.GetAvatarUrl())
                        .WithText($"{bot}");
                    });
                    Embed embedTwo = embedbuilderTwo.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);
                }
                else
                {
                    var embedbuilderTwo = new EmbedBuilder()
                    .WithColor(Color.DarkGreen)
                    .WithAuthor($"{userThatJoined.Username} has appeared.", userThatJoined.GetAvatarUrl())
                    .WithDescription($"{joinEmote} {userThatJoined.Mention} *(ID: {userThatJoined.Id})*")
                    .AddField("Account Created On", $"{userThatJoined.CreatedAt.UtcDateTime.ToString("D")} *(less than an hour)*")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithIconUrl(bot.GetAvatarUrl())
                        .WithText($"{bot}");
                    });
                    Embed embedTwo = embedbuilderTwo.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);
                }

                Embed embed = embedBuilder.Build();

                await welcomeChannel.SendMessageAsync(embed: embed);

            } else {

                var embedBuilder = new EmbedBuilder()
                .WithColor(Color.DarkPurple)
                .WithAuthor($"{userThatJoined.DisplayName}", userThatJoined.GetDefaultAvatarUrl())
                .WithThumbnailUrl(serverIconUrl)
                .WithTitle($"Welcome to {serverName} !")
                .WithDescription($"- get your roles in <#{rolesChannel.Id}>\n- please read the rules <#{rulesChannel.Id}>\n")
                .WithFooter($"{partyEmoji} Membership Count: {memberCount} {partyEmoji}")
                .WithCurrentTimestamp();


                //announce user that joined and information in join-leave-logs
                var loggingChannel = _client.GetChannel(965699250522558566) as SocketTextChannel;
                IEmote joinEmote = Emote.Parse("<a:join:993953953832251542>");
                var bot = _client.CurrentUser;

                DateTime currentDate = DateTime.UtcNow;

                // Calculate years, months, and days since account creation
                TimeSpan accountAge = currentDate - userThatJoined.CreatedAt.UtcDateTime;
                int accountYears = (int)(accountAge.TotalDays / 365);
                int accountMonths = (int)((accountAge.TotalDays % 365) / 30);
                int accountDays = (int)(accountAge.TotalDays % 30);
                int accountHours = (int)accountAge.TotalHours;

                if (accountYears > 0){

                    var embedbuilderTwo = new EmbedBuilder()
                    .WithColor(Color.DarkGreen)
                    .WithAuthor($"{userThatJoined.Username} has appeared.", userThatJoined.GetDefaultAvatarUrl())
                    .WithDescription($"{joinEmote} {userThatJoined.Mention} *(ID: {userThatJoined.Id})*")
                    .AddField("Account Created On", $"{userThatJoined.CreatedAt.UtcDateTime.ToString("D")} *({accountYears} years, {accountMonths} months, {accountDays} days)*")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithIconUrl(bot.GetAvatarUrl())
                        .WithText($"{bot}");
                    });
                    Embed embedTwo = embedbuilderTwo.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);

                }else if (accountMonths > 0){

                    var embedbuilderTwo = new EmbedBuilder()
                    .WithColor(Color.DarkGreen)
                    .WithAuthor($"{userThatJoined.Username} has appeared.", userThatJoined.GetDefaultAvatarUrl())
                    .WithDescription($"{joinEmote} {userThatJoined.Mention} *(ID: {userThatJoined.Id})*")
                    .AddField("Account Created On", $"{userThatJoined.CreatedAt.UtcDateTime.ToString("D")} *({accountMonths} months, {accountDays} days)*")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithIconUrl(bot.GetAvatarUrl())
                        .WithText($"{bot}");
                    });
                    Embed embedTwo = embedbuilderTwo.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);

                }else if (accountDays > 0){

                    var embedbuilderTwo = new EmbedBuilder()
                    .WithColor(Color.DarkGreen)
                    .WithAuthor($"{userThatJoined.Username} has appeared.", userThatJoined.GetDefaultAvatarUrl())
                    .WithDescription($"{joinEmote} {userThatJoined.Mention} *(ID: {userThatJoined.Id})*")
                    .AddField("Account Created On", $"{userThatJoined.CreatedAt.UtcDateTime.ToString("D")} *({accountDays} days)*")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithIconUrl(bot.GetAvatarUrl())
                        .WithText($"{bot}");
                    });
                    Embed embedTwo = embedbuilderTwo.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);

                }else if (accountHours > 0){
                    var embedbuilderTwo = new EmbedBuilder()
                    .WithColor(Color.DarkGreen)
                    .WithAuthor($"{userThatJoined.Username} has appeared.", userThatJoined.GetDefaultAvatarUrl())
                    .WithDescription($"{joinEmote} {userThatJoined.Mention} *(ID: {userThatJoined.Id})*")
                    .AddField("Account Created On", $"{userThatJoined.CreatedAt.UtcDateTime.ToString("D")} *({accountHours} hours)*")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithIconUrl(bot.GetAvatarUrl())
                        .WithText($"{bot}");
                    });
                    Embed embedTwo = embedbuilderTwo.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);
                }else{
                    var embedbuilderTwo = new EmbedBuilder()
                    .WithColor(Color.DarkGreen)
                    .WithAuthor($"{userThatJoined.Username} has appeared.", userThatJoined.GetDefaultAvatarUrl())
                    .WithDescription($"{joinEmote} {userThatJoined.Mention} *(ID: {userThatJoined.Id})*")
                    .AddField("Account Created On", $"{userThatJoined.CreatedAt.UtcDateTime.ToString("D")} *(less than an hour)*")
                    .WithCurrentTimestamp()
                    .WithFooter(footer => {
                        footer
                        .WithIconUrl(bot.GetAvatarUrl())
                        .WithText($"{bot}");
                    });
                    Embed embedTwo = embedbuilderTwo.Build();
                    await loggingChannel.SendMessageAsync(embed: embedTwo);
                }

                Embed embed = embedBuilder.Build();

                await welcomeChannel.SendMessageAsync(embed: embed);

            }

        }

        public async Task LeaveLogging(SocketGuild thisGuild, SocketUser userLeave){
            var loggingChannel = _client.GetChannel(965699250522558566) as SocketTextChannel;
            IEmote leaveEmote = Emote.Parse("<a:leave:993953885339267173>");
            var bot = _client.CurrentUser;

            var embedbuilder = new EmbedBuilder()
                .WithColor(Color.DarkRed)
                .WithDescription($"{leaveEmote}** {userLeave.Username} has vanished**.")
                .WithCurrentTimestamp()
                .WithFooter(footer => {
                    footer
                    .WithIconUrl(bot.GetAvatarUrl())
                    .WithText($"{bot}");
                });

            Embed embed = embedbuilder.Build();

            await loggingChannel.SendMessageAsync(embed: embed);
        }
    }
}
