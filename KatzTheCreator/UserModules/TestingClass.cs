using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Net;

namespace KatzTheCreator.UserModules
{
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
        public async Task UploadEmoji(SocketGuildUser user){
            var grabAvatarURL = user.GetAvatarUrl();
            var uploadImageChannel = Context.Guild.GetChannel(988184539455172688) as ISocketMessageChannel;

            await uploadImageChannel.SendFileAsync(grabAvatarURL);
            var imageMessage = await uploadImageChannel.GetMessagesAsync(1).FlattenAsync();
            var attachment = imageMessage.Last().Attachments.FirstOrDefault();
            //await Context.Guild.CreateEmoteAsync("emojiName", attachment);
            


                
        }
    }
}
