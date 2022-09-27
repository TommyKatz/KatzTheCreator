using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.Config
{
    public class CommandHandler{
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services){

            _commands = commands;
            _client = client;
            _services = services;

            _client.MessageReceived += HandleCommandAsync;
            _client.ButtonExecuted += RegisterButtonHandler;

            _client.UserJoined += AnnounceJoinedUser;
            _client.UserJoined += JoinLogging;
            _client.UserLeft += LeaveLogging;
        }

        public async Task RegisterCommandAsync(){
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task RegisterButtonHandler(SocketMessageComponent component){

            var rUser = component.User as SocketGuildUser;
            ulong scgRoleID = 965700697679077406;
            ulong killerRoleID = 965702660542062653;
            var getSCGRole = rUser.Guild.Roles.FirstOrDefault(x => x.Id == 965700697679077406);
            var getKillerRole = rUser.Guild.Roles.FirstOrDefault(x => x.Id == 965702660542062653);

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
            }
        }

        private async Task HandleCommandAsync(SocketMessage arg){
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);

            if (message.Author.IsBot) return;

            int argPos = 0;

            if (message.HasStringPrefix("?", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                var waitTimeSeven = 7000;
                var rUser = message.Author.Mention;

                if (!result.IsSuccess) Console.Write(result.ErrorReason);

                if (result.Error.Equals(CommandError.UnmetPrecondition)){
                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithDescription($"{rUser}, unable to perform this action; {result.ErrorReason}");
                    Embed embed = embedBuilder.Build();
                    var botReply = await message.Channel.SendMessageAsync(embed: embed);
                    await message.DeleteAsync();
                    await Task.Delay(waitTimeSeven);
                    await botReply.DeleteAsync();
                }
                
                if (result.Error.Equals(CommandError.UnknownCommand)){

                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithDescription($"{rUser}, this is an invalid command; **Try again** or type ``?help`` to see the commands available");
                    Embed embed = embedBuilder.Build();
                    var botReply = await message.Channel.SendMessageAsync(embed: embed);
                    await message.DeleteAsync();
                    await Task.Delay(waitTimeSeven);
                    await botReply.DeleteAsync();
                }
                if (result.Error.Equals(CommandError.ObjectNotFound)){

                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithDescription($"{rUser}, unable to perform this action; {result.ErrorReason}");
                    Embed embed = embedBuilder.Build();
                    var botReply = await message.Channel.SendMessageAsync(embed: embed);
                    await message.DeleteAsync();
                    await Task.Delay(waitTimeSeven);
                    await botReply.DeleteAsync();
                }

                if (result.Error.Equals(CommandError.ParseFailed)){
                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithDescription($"{rUser}, you used this command incorrectly; **Try again** or type ``?help`` to see the commands available, and how to use them.");
                    Embed embed = embedBuilder.Build();
                    var botReply = await message.Channel.SendMessageAsync(embed: embed);
                    await message.DeleteAsync();
                    await Task.Delay(waitTimeSeven);
                    await botReply.DeleteAsync();
                }

                if (result.Error.Equals(CommandError.Exception)){
                    var embedBuilder = new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithDescription($"{rUser}, unable to perform this action; {result.ErrorReason}");
                    Embed embed = embedBuilder.Build();
                    var botReply = await message.Channel.SendMessageAsync(embed: embed);
                    await message.DeleteAsync();
                    await Task.Delay(waitTimeSeven);
                    await botReply.DeleteAsync();
                }
            } 
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
