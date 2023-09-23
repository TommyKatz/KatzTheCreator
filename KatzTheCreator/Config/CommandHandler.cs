using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

namespace KatzTheCreator.Config{
    public class CommandHandler{
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        
        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services){

            _commands = commands;
            _client = client;
            _services = services;

            _client.MessageReceived += HandleCommandAsync;
        }

        public async Task RegisterCommandAsync(){
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg){
            if (string.IsNullOrEmpty(arg.Content)) return;
            var message = arg as SocketUserMessage;

            if (message.Author.IsBot || message.Channel.GetChannelType() == ChannelType.DM) return;

            var context = new SocketCommandContext(_client, message);

            int argPos = 0;

            if (message.HasStringPrefix("?", ref argPos)){
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                var rUser = message.Author;

                if (!result.IsSuccess) Console.Write(result.ErrorReason);

                List<string> punctuation = new List<string>();
                punctuation.Add("?");
                punctuation.Add("!");
                punctuation.Add(".");
                IEnumerable<string> allowedPunctuations = punctuation;

                if (result.Error.Equals(CommandError.UnmetPrecondition)){
                    await message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" + 
                    $"***Uh oh! Something went wrong...***\n\nYou do not have permission to use this.");
                }

                if (result.Error.Equals(CommandError.UnknownCommand) && allowedPunctuations.Contains(message.Content)){ // allows punct. to be used without deleting and throwing the user an error
                    await message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" + 
                    "***Uh oh! Something went wrong...***\n\nThis is an invalid command; **Try again** or type ``?help`` to see the commands available");
                }
                if (result.Error.Equals(CommandError.ObjectNotFound)){
                    await message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" + 
                    $"***Uh oh! Something went wrong...***\n\nUnable to perform this action; {result.ErrorReason}");
                }

                if (result.Error.Equals(CommandError.ParseFailed)){
                    await message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" + 
                    "***Uh oh! Something went wrong...***\n\nYou used this command incorrectly; **Try again** or type ``?help`` to see the commands available, and how to use them.");
                }

                if (result.Error.Equals(CommandError.Exception)){
                    await message.DeleteAsync();
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                    $"***Uh oh! Something went wrong...***\n\nUnable to perform this action; {result.ErrorReason}");
                }
            } 
        }
    }
}
