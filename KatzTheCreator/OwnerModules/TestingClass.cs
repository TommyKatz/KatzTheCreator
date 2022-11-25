using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Net;

namespace KatzTheCreator.OwnerModules
{
    public class TestingClass : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        [RequireOwner]
        public async Task GrabbingUserRoleColor(SocketGuildUser user)
        {

            // filters out any roles that are default color
            var filterOutDefault = user.Roles.Where(r => r.Color != Color.Default);

            // grabs the highest role and the color for it
            var userHighestRoleColor = filterOutDefault.MaxBy(r => r.Position).Color;

            // returns the hex code for it
            await ReplyAsync($"This user's color is {userHighestRoleColor}");

        }

        [Command("test2")]
        [RequireOwner]
        public async Task UploadEmoji(SocketGuildUser user)
        {
            var grabAvatarURL = user.GetAvatarUrl();
            var uploadImageChannel = Context.Guild.GetChannel(988184539455172688) as ISocketMessageChannel;

            await uploadImageChannel.SendFileAsync(grabAvatarURL);
            var imageMessage = await uploadImageChannel.GetMessagesAsync(1).FlattenAsync();
            var attachment = imageMessage.Last().Attachments.FirstOrDefault();
            //await Context.Guild.CreateEmoteAsync("emojiName", attachment);        
        }

        [Command("test 3")]
        [RequireOwner]
        public async Task TestingClassEx(){

        }
    }
}
