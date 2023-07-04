using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace KatzTheCreator.Config{
    public class Program{
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private InteractionService _interaction;
        public async Task RunBotAsync(){

            var config = new DiscordSocketConfig {
                LogLevel = LogSeverity.Debug,
                AlwaysDownloadUsers = true,
                MessageCacheSize = 100,
                GatewayIntents = GatewayIntents.GuildMessages | GatewayIntents.MessageContent | GatewayIntents.GuildMembers | GatewayIntents.GuildBans | GatewayIntents.GuildPresences |
                GatewayIntents.GuildVoiceStates | GatewayIntents.GuildMessageReactions | GatewayIntents.DirectMessages | GatewayIntents.GuildScheduledEvents |
                GatewayIntents.Guilds | GatewayIntents.GuildIntegrations | GatewayIntents.GuildEmojis | GatewayIntents.GuildInvites
                
            };

            _client = new DiscordSocketClient(config);

            _commands = new CommandService();

            _interaction = new InteractionService(_client);

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton(_interaction)
                .AddSingleton<InteractionHandler>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<UpdateHandler>()
                .BuildServiceProvider();

            string token = File.ReadAllText("token.txt");

            await _services.GetRequiredService<CommandHandler>().RegisterCommandAsync();
            await _services.GetRequiredService<UpdateHandler>().RegisterCommandAsync();
            await _services.GetRequiredService<InteractionHandler>().InitalizeAsync();

            await _client.SetGameAsync("Bubba Face Camp ;(", type: ActivityType.Watching);

            _client.Log += async (LogMessage msg) => { Console.WriteLine(msg.Message); };
            _interaction.Log += async (LogMessage msg) => { Console.WriteLine(msg.Message); };

            _client.Ready += async () =>{
                Console.WriteLine($"Connected as -> [{_client.CurrentUser}]");
                await _interaction.RegisterCommandsGloballyAsync(deleteMissing: true);
                Console.WriteLine("Commands have been registred");

            };

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }
    }
}