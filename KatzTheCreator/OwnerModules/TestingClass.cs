using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Drawing;

namespace KatzTheCreator.OwnerModules
{
    public class TestingClass : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        [RequireOwner]
        public async Task GrabbingUserRoleColor(SocketGuildUser user)
        {

            // filters out any roles that are default color
            var filterOutDefault = user.Roles.Where(r => r.Color != Discord.Color.Default);

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

        [Command("test3")]
        [RequireOwner]
        public async Task TestingClassEx(){
            await Context.Message.DeleteAsync();

            var embedColor = Context.Guild.Roles.FirstOrDefault(x => x.Id == 988179216791113780).Color;

            var builder = new EmbedBuilder()
            .WithColor(embedColor)
            .WithImageUrl("https://i.imgur.com/9USiU6L.png")
            .WithTitle("PVC INTERFACE")
            .WithDescription("These commands can be used to manage private voice channels - for more details on each command, type `?ihelp`.")
            .WithFooter(footer => {
                footer
                .WithText("• These commands can only be used in the commands channel.\n• All commands must start with a `?` or they will fail.\n• Misuse could result in penalization.");
             });
            var embed = builder.Build();

            await ReplyAsync(embed: embed);
            
        }
    }
}
