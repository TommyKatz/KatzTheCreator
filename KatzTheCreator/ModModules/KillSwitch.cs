using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.ModModules
{
    public class KillSwitch : ModuleBase<SocketCommandContext> {
        [Command("killswitch")]
        public async Task BotShutdown(){
            var rUser = Context.User as SocketGuildUser;

            List<ulong> discordId = new List<ulong>();
            discordId.Add(135143527767080960); // katz
            discordId.Add(968134907270402058); // witchdoctor
            discordId.Add(153643067516125185); // lamb
            IEnumerable<ulong> allowedIds = discordId;

            if (!allowedIds.Contains(rUser.Id)){
                await Context.Message.DeleteAsync();
                return;
            } else {
                await Context.Message.DeleteAsync();
                await ReplyAsync($"{rUser.Mention}, **Killswitch Activated:** Powering down... goodbye.");
                Environment.Exit(0);
            }
        }
    }
}
