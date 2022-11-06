using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KatzTheCreator.ModModules{
    public class CodeAlerts : ModuleBase<SocketCommandContext>{
        [Command("newcode")]
        public async Task CodeEmbed(char type = default, int amount = default, [Remainder] string content = null){
            var rUser = Context.User as SocketGuildUser;
            var announceChannel = Context.Guild.GetChannel(1009837099085742110) as SocketTextChannel;

            List<ulong> discordId = new List<ulong>();
            discordId.Add(135143527767080960); // katz
            discordId.Add(968134907270402058); // witchdoctor
            discordId.Add(527473869439762434); // harpy
            IEnumerable<ulong> allowedIds = discordId;

            List<char> charType = new List<char>();
            charType.Add('B'); // Bloodpoints
            charType.Add('S'); // Iridescent Shards
            charType.Add('F'); // Rift Fragments
            charType.Add('C'); // Charms
            IEnumerable<char> allowedChars = charType;

            if (!allowedIds.Contains(rUser.Id)){
                await Context.Message.DeleteAsync();

            } else {
                await Context.Message.DeleteAsync();

                if (type == default || amount == default || content == null){
                    await rUser.SendMessageAsync("---------------------------------------------------------------------\n" + 
                    "***Uh oh! Something went wrong...***\n\nToo few arguments provided, must contain 3: ?newcode `B` <-- type of reward `25000` <-- amount of reward `TestCode1` <-- content.\n\n```Example: ?newcode B 75000 TestCode1, TestCode2```");
                    return;
                } else {
                    string[] subs = content.Split(", ");
                    var adjustedSubs = from sub in subs select sub.Replace($"{sub}", $"\" **{sub}** \"");

                    try{
                        var messages = await announceChannel.GetMessagesAsync(1).FlattenAsync();
                        var latestCommand = Context.Message.Timestamp.DateTime;
                        var latestMessage = messages.Last().Timestamp.DateTime;

                        if ((latestCommand - latestMessage).TotalMinutes >= 30){
                            if (!allowedChars.Contains(type)){
                                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                                "***Uh oh! Something went wrong...***\n\nThis reward doesn't exist, here is the ones that do: `B` = Bloodpoints `S` = Iridescent Shards `F` = Rift Fragments *(Case Sensitive)*");
                                return;
                            }else if (type.Equals('B')){
                                await announceChannel.SendMessageAsync("<@&1036799014508703794>");

                                var builder = new EmbedBuilder()
                                    .WithColor(Color.DarkRed)
                                    .WithCurrentTimestamp()
                                    .WithThumbnailUrl("https://i.imgur.com/vEP5SNh.png")
                                    .WithTitle($"Enter code(s) below to receive {amount} Bloodpoints !")
                                    .WithDescription($"{string.Join("\n\n", adjustedSubs)}")
                                    .WithFooter(footer => {
                                        footer
                                        .WithText($"Published by {rUser}")
                                        .WithIconUrl(rUser.GetAvatarUrl());
                                    });
                                Embed embed = builder.Build();
                                await announceChannel.SendMessageAsync(embed: embed);
                            }else if (type.Equals('S')){
                                await announceChannel.SendMessageAsync("<@&1036799014508703794>");

                                var builder = new EmbedBuilder()
                                    .WithColor(Color.DarkPurple)
                                    .WithCurrentTimestamp()
                                    .WithThumbnailUrl("https://i.imgur.com/413V5Ze.png")
                                    .WithTitle($"Enter code(s) below to receive {amount} Iridescent Shards !")
                                    .WithDescription($"{string.Join("\n\n", adjustedSubs)}")
                                    .WithFooter(footer => {
                                        footer
                                        .WithText($"Published by {rUser}")
                                        .WithIconUrl(rUser.GetAvatarUrl());
                                    });
                                Embed embed = builder.Build();
                                await announceChannel.SendMessageAsync(embed: embed);

                            }else if (type.Equals('F')){
                                await announceChannel.SendMessageAsync("<@&1036799014508703794>");

                                var builder = new EmbedBuilder()
                                    .WithColor(Color.DarkBlue)
                                    .WithCurrentTimestamp()
                                    .WithThumbnailUrl("https://i.imgur.com/dBMrFhP.png")
                                    .WithTitle($"Enter code(s) below to receive {amount} Rift Fragments !")
                                    .WithDescription($"{string.Join("\n\n", adjustedSubs)}")
                                    .WithFooter(footer => {
                                        footer
                                        .WithText($"Published by {rUser}")
                                        .WithIconUrl(rUser.GetAvatarUrl());
                                    });
                                Embed embed = builder.Build();
                                await announceChannel.SendMessageAsync(embed: embed);

                            }else if (type.Equals('C')){
                                await announceChannel.SendMessageAsync("<@&1036799014508703794>");

                                var builder = new EmbedBuilder()
                                    .WithColor(Color.Teal)
                                    .WithCurrentTimestamp()
                                    .WithThumbnailUrl("https://i.imgur.com/Fa5jJJm.png")
                                    .WithTitle($"Enter code(s) below to receive newly added Charm(s) !")
                                    .WithDescription($"{string.Join("\n\n", adjustedSubs)}")
                                    .WithFooter(footer => {
                                        footer
                                        .WithText($"Published by {rUser}")
                                        .WithIconUrl(rUser.GetAvatarUrl());
                                    });
                                Embed embed = builder.Build();
                                await announceChannel.SendMessageAsync(embed: embed);
                            }
                        }else{

                            if (!allowedChars.Contains(type)){
                                await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                                "***Uh oh! Something went wrong...***\n\nThis reward doesn't exist, here is the ones that do: `B` = Bloodpoints `S` = Iridescent Shards `F` = Rift Fragments *(Case Sensitive)*");
                            }else if (type.Equals('B')){
                                var builder = new EmbedBuilder()
                                    .WithColor(Color.DarkRed)
                                    .WithCurrentTimestamp()
                                    .WithThumbnailUrl("https://i.imgur.com/vEP5SNh.png")
                                    .WithTitle($"Enter code(s) below to receive {amount} Bloodpoints !")
                                    .WithDescription($"{string.Join("\n\n", adjustedSubs)}")
                                    .WithFooter(footer => {
                                        footer
                                        .WithText($"Published by {rUser}")
                                        .WithIconUrl(rUser.GetAvatarUrl());
                                    });
                                Embed embed = builder.Build();
                                await announceChannel.SendMessageAsync(embed: embed);

                            }else if (type.Equals('S')){
                                var builder = new EmbedBuilder()
                                    .WithColor(Color.DarkPurple)
                                    .WithCurrentTimestamp()
                                    .WithThumbnailUrl("https://i.imgur.com/413V5Ze.png")
                                    .WithTitle($"Enter code(s) below to receive {amount} Iridescent Shards !")
                                    .WithDescription($"{string.Join("\n\n", adjustedSubs)}")
                                    .WithFooter(footer => {
                                        footer
                                        .WithText($"Published by {rUser}")
                                        .WithIconUrl(rUser.GetAvatarUrl());
                                    });
                                Embed embed = builder.Build();
                                await announceChannel.SendMessageAsync(embed: embed);

                            }else if (type.Equals('F')){
                                var builder = new EmbedBuilder()
                                    .WithColor(Color.DarkBlue)
                                    .WithCurrentTimestamp()
                                    .WithThumbnailUrl("https://i.imgur.com/dBMrFhP.png")
                                    .WithTitle($"Enter code(s) below to receive {amount} Rift Fragments !")
                                    .WithDescription($"{string.Join("\n\n", adjustedSubs)}")
                                    .WithFooter(footer => {
                                        footer
                                        .WithText($"Published by {rUser}")
                                        .WithIconUrl(rUser.GetAvatarUrl());
                                    });
                                Embed embed = builder.Build();
                                await announceChannel.SendMessageAsync(embed: embed);

                            }else if (type.Equals('C')){
                                var builder = new EmbedBuilder()
                                    .WithColor(Color.Teal)
                                    .WithCurrentTimestamp()
                                    .WithThumbnailUrl("https://i.imgur.com/Fa5jJJm.png")
                                    .WithTitle($"Enter code(s) below to receive newly added Charm(s) !")
                                    .WithDescription($"{string.Join("\n\n", adjustedSubs)}")
                                    .WithFooter(footer => {
                                        footer
                                        .WithText($"Published by {rUser}")
                                        .WithIconUrl(rUser.GetAvatarUrl());
                                    });
                                Embed embed = builder.Build();
                                await announceChannel.SendMessageAsync(embed: embed);
                            }
                        }
                    }catch (Exception){

                        if (!allowedChars.Contains(type)){
                            await rUser.SendMessageAsync("---------------------------------------------------------------------\n" +
                            "***Uh oh! Something went wrong...***\n\nThis reward doesn't exist, here is the ones that do: `B` = Bloodpoints `S` = Iridescent Shards `F` = Rift Fragments *(Case Sensitive)*");
                            return;
                        }else if (type.Equals('B')){
                            await announceChannel.SendMessageAsync("<@&1036799014508703794>");

                            var builder = new EmbedBuilder()
                                .WithColor(Color.DarkRed)
                                .WithCurrentTimestamp()
                                .WithThumbnailUrl("https://i.imgur.com/vEP5SNh.png")
                                .WithTitle($"Enter code(s) below to receive {amount} Bloodpoints !")
                                .WithDescription($"{string.Join("\n\n", adjustedSubs)}")
                                .WithFooter(footer => {
                                    footer
                                    .WithText($"Published by {rUser}")
                                    .WithIconUrl(rUser.GetAvatarUrl());
                                });
                            Embed embed = builder.Build();
                            await announceChannel.SendMessageAsync(embed: embed);

                        }else if (type.Equals('S')){
                            await announceChannel.SendMessageAsync("<@&1036799014508703794>");

                            var builder = new EmbedBuilder()
                                .WithColor(Color.DarkPurple)
                                .WithCurrentTimestamp()
                                .WithThumbnailUrl("https://i.imgur.com/413V5Ze.png")
                                .WithTitle($"Enter code(s) below to receive {amount} Iridescent Shards !")
                                .WithDescription($"{string.Join("\n\n", adjustedSubs)}")
                                .WithFooter(footer => {
                                    footer
                                    .WithText($"Published by {rUser}")
                                    .WithIconUrl(rUser.GetAvatarUrl());
                                });
                            Embed embed = builder.Build();
                            await announceChannel.SendMessageAsync(embed: embed);

                        }else if (type.Equals('F')){
                            await announceChannel.SendMessageAsync("<@&1036799014508703794>");

                            var builder = new EmbedBuilder()
                                .WithColor(Color.DarkBlue)
                                .WithCurrentTimestamp()
                                .WithThumbnailUrl("https://i.imgur.com/dBMrFhP.png")
                                .WithTitle($"Enter code(s) below to receive {amount} Rift Fragments !")
                                .WithDescription($"{string.Join("\n\n", adjustedSubs)}")
                                .WithFooter(footer => {
                                    footer
                                    .WithText($"Published by {rUser}")
                                    .WithIconUrl(rUser.GetAvatarUrl());
                                });
                            Embed embed = builder.Build();
                            await announceChannel.SendMessageAsync(embed: embed);
                        }else if (type.Equals('C')){
                            await announceChannel.SendMessageAsync("<@&1036799014508703794>");

                            var builder = new EmbedBuilder()
                                .WithColor(Color.Teal)
                                .WithCurrentTimestamp()
                                .WithThumbnailUrl("https://i.imgur.com/Fa5jJJm.png")
                                .WithTitle($"Enter code(s) below to receive newly added Charm(s) !")
                                .WithDescription($"{string.Join("\n\n", adjustedSubs)}")
                                .WithFooter(footer => {
                                    footer
                                    .WithText($"Published by {rUser}")
                                    .WithIconUrl(rUser.GetAvatarUrl());
                                });
                            Embed embed = builder.Build();
                            await announceChannel.SendMessageAsync(embed: embed);
                        }
                    }
                }
            } 
        }
    }
}
