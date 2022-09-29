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
        }

        public async Task RegisterCommandAsync(){
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg){
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);

            if (message.Author.IsBot) return;

            int argPos = 0;

            if (message.HasStringPrefix("?", ref argPos)){
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
    }
}
