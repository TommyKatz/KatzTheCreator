using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace KatzTheCreator.Config{
    public class Program{
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private SocketMessageComponent _buttons;
        public async Task RunBotAsync(){

            var config = new DiscordSocketConfig {
                LogLevel = LogSeverity.Debug,
                AlwaysDownloadUsers = true,
                MessageCacheSize = 100,
                GatewayIntents = GatewayIntents.GuildMessages | GatewayIntents.GuildMembers | GatewayIntents.GuildBans | GatewayIntents.GuildPresences |
                GatewayIntents.GuildVoiceStates | GatewayIntents.GuildMessageReactions | GatewayIntents.DirectMessages | GatewayIntents.GuildScheduledEvents |
                GatewayIntents.Guilds | GatewayIntents.GuildIntegrations
                
            };
            
            

            _client = new DiscordSocketClient(config);

            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton<CommandHandler>()
                .BuildServiceProvider();

            _client.Log += _client_Log;

            string token = File.ReadAllText("token.txt");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await _services.GetRequiredService<CommandHandler>().RegisterCommandAsync();

            await _client.SetGameAsync("with your heart");

            await Task.Delay(-1);
        }

        private Task _client_Log(LogMessage msg){
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync(){
            Console.WriteLine($"Connected as -> [{_client.CurrentUser}]");
            return Task.CompletedTask;
        }
    }
}