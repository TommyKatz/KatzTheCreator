﻿using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace KatzTheCreator.OwnerModules
{
    /*public class TestingClass : InteractionModuleBase<SocketInteractionContext>{
        [SlashCommand("test", "returns the color of a user")]
        [Discord.Interactions.RequireOwner]
        public async Task GrabbingUserRoleColor(SocketGuildUser user)
        {

            // filters out any roles that are default color
            var filterOutDefault = user.Roles.Where(r => r.Color != Discord.Color.Default);

            // grabs the highest role and the color for it
            var userHighestRoleColor = filterOutDefault.MaxBy(r => r.Position).Color;

            // returns the hex code for it
            await RespondAsync($"This user's color is {userHighestRoleColor}");

        }

        [SlashCommand("test2", "trying to download a user's pfp")]
        [Discord.Interactions.RequireOwner]
        public async Task UploadEmoji(SocketGuildUser user)
        {
            var grabAvatarURL = user.GetAvatarUrl();
            var uploadImageChannel = Context.Guild.GetChannel(988184539455172688) as ISocketMessageChannel;

            await uploadImageChannel.SendFileAsync(grabAvatarURL);
            var imageMessage = await uploadImageChannel.GetMessagesAsync(1).FlattenAsync();
            var attachment = imageMessage.Last().Attachments.FirstOrDefault();
            //await Context.Guild.CreateEmoteAsync("emojiName", attachment);        
        }

        [SlashCommand("test3", "pvc embed")]
        [Discord.Interactions.RequireOwner]
        public async Task TestingClassEx(){

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

            await RespondAsync(embed: embed);
            
        }

        [SlashCommand("21", "play 21 the game")]
        [Discord.Interactions.RequireOwner]
        public async Task TwentyOneTheGame(){

            List<string> cardType = new List<string>();
            cardType.Add("One"); cardType.Add("Two"); // 0, 1
            cardType.Add("Three"); cardType.Add("Four"); // 2, 3
            cardType.Add("Five"); cardType.Add("Six"); // 4, 5
            cardType.Add("Seven"); cardType.Add("Eight"); // 6, 7
            cardType.Add("Nine"); cardType.Add("Ten"); // 8, 9
            cardType.Add("Jack"); cardType.Add("Queen"); // 10, 11
            cardType.Add("King"); cardType.Add("Ace"); // 12, 13
            IEnumerable<string> cardDeck = cardType;


            Random rnd = new Random();
            var cardNumShuffle = cardDeck.OrderBy(c => rnd.Next()).ToList();
            Console.WriteLine($"{String.Join(", ", cardNumShuffle)}");

            var pcOne = cardNumShuffle.ElementAt(0);
            var pcTwo = cardNumShuffle.ElementAt(1);

            var dcOne = cardNumShuffle.ElementAt(2);
            var dcTwo = cardNumShuffle.ElementAt(3);

            // prompt the player to hit or keep their current hand
            var builderTwo = new ComponentBuilder()
                .WithButton("Hit", "Hit", ButtonStyle.Success)
                .WithButton("Keep", "Keep", ButtonStyle.Danger);

            await RespondAsync($"Your First Card: {pcOne} | Your Second Card: {pcTwo}\nThe Dealer's Second Card: {dcTwo}", components: builderTwo.Build());

        }



    }*/
}
