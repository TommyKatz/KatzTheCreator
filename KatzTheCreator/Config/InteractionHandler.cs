﻿using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;

namespace KatzTheCreator.Config
{
    public class InteractionHandler{
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _commands;
        private readonly IServiceProvider _services;

        public InteractionHandler(DiscordSocketClient client, InteractionService commands, IServiceProvider services){
            _client = client;
            _commands = commands;
            _services = services;
        }

        public async Task InitalizeAsync(){
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            _client.InteractionCreated += HandleInteraction;
        }

        public async Task HandleInteraction(SocketInteraction arg){
            try{

                var ctx = new SocketInteractionContext(_client, arg);
                await _commands.ExecuteCommandAsync(ctx, _services);

            } catch (Exception ex){

                Console.WriteLine(ex.ToString());
            }
            
        }

    }
}
