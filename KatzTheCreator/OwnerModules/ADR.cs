using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.OwnerModules{
    public class ADR : ModuleBase<SocketCommandContext>{
    [Command("ADR")]
    [RequireOwner]

        public async Task KeepADR(int amount = default){

            var devChannel = Context.Guild.GetChannel(988184539455172688) as SocketTextChannel;

            await devChannel.SendMessageAsync("/av 135143527767080960");

        }

    }
}