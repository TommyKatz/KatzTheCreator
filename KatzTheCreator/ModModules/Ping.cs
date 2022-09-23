using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.ModModules
{
    public class Ping : ModuleBase<SocketCommandContext>{
        [Command("ping")]
        [Alias("latency")]
        [Summary("Shows the websocket connection's latency and time it takes for me send a message.")]
        public async Task PingAsync()
        {
            var rUser = Context.User as SocketGuildUser;
            var directorRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == 965695483068686367);

            if (rUser.Roles.Contains(directorRole)){

                // start a new stopwatch to measure the time it takes for us to send a message
                var sw = Stopwatch.StartNew();
                // send the message and store it for later modification
                var msg = await ReplyAsync($"**Websocket latency**: *{Context.Client.Latency}ms*\n" + "**Response**: *...*");
                // pause the stopwatch
                sw.Stop();

                // modify the message we sent earlier to display measured time
                await msg.ModifyAsync(x => x.Content = $"**Websocket latency**: *{Context.Client.Latency}ms*\n" + $"**Response**: *{sw.Elapsed.TotalMilliseconds}ms*");
            } else {
                await Context.Message.DeleteAsync();
            }
        }
    }
}