using System.Reflection;
using Discord.WebSocket;
using Discord;
using Discord.Commands;

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

        public async Task MessageOpsAsync(SocketMessage msg){
            if (msg.Author.IsBot) return;

            Emote hmmEmote = Emote.Parse("<:hmm:1025075030540959775>");
            Random rnd = new Random();
            int number = rnd.Next(1, 101);

            if (number <= 4){ // 4% chance
                await msg.AddReactionAsync(hmmEmote);
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
