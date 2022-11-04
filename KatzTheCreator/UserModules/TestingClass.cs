using Discord;
using Discord.Commands;
using Discord.Interactions;
using KatzTheCreator.ModModules;
using System.Reactive.Linq;

namespace KatzTheCreator.UserModules{
    public class TestingClass : ModuleBase<SocketCommandContext>{
        [Command("test")]
        public async Task ReactionsEmbed(){
            List<string> passionItems = new List<string>();
            passionItems.Add("Director");
            passionItems.Add("Religious Critic");
            passionItems.Add("Political Enthusiast");
            IEnumerable<string> aboutMe = passionItems;

            await Context.Channel.SendMessageAsync($"{string.Join(", ", aboutMe)}");
        }

        [Command("test2")]
        public async Task TestingMethod()
        {
            await ReplyAsync("wow, you have permission !");
        }
    }
}
