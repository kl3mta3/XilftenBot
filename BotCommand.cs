using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;


namespace XilftenBot
{

    /// <summary>
    ///  Generic Application commands for managing the bot and requests
    /// </summary>
    internal class BotCommand : ApplicationCommandModule
    {
        // Generic Slash Commands
        [SlashCommand("Help", "Send the instructions in PM!")]
        public async Task Help(InteractionContext ctx)
        {

            var user = ctx.Member;

            var em = new DiscordEmbedBuilder
            {
                Color = DiscordColor.CornflowerBlue,
                Title = $"Xilften Help",
                Description = $"How to add Shows and Movies with Discord."
            };

            em.AddField("To add Tv Shows ", "Type /TV provied the name and year in the this format <Name of Show(Year)>", false);

            em.AddField("To add Movies", "Type /Movie provied the name and year in the this format <Name of Movie(Year)>", false);

            em.AddField("Admin Commands", "Admin access on the server is required to use these commands", false);

            em.AddField("/RestartSonarr", "Resets the Sonarr Server", false);
            em.AddField("/RestartRadarr", "Resets the Radarr Server", false);

            em.AddField("/GetRecent", "Choose 7 days, 30 days, or all'0' and recieve the requests in a PM.", false);
            em.AddField("/RemoveRecent", "Choose 7 days, 30 days, or all'0' and remove requests results in that period.", false);
            em.AddField("/UserRequests", "Choose 7 days, 30 days, or all'0', Provide a Username and get the requested results in a PM.", false);
            em.Build();

            await user.SendMessageAsync(embed: em);

        }

        //Gets the recent requests with the time frame(7 days,30 days, all time)
        [SlashCommand("GetRecent", "Pm user the requests made in a period of time.!")]
        public async Task Recent(InteractionContext ctx,
            [Choice("7 days", 7)]
            [Choice("30 days", 30)]
            [Choice("all time", 0)]
            [Option("searchTime", "Days to Search")] long searchTime = 0)
        {
        
            var user = ctx.Member;
            //If the user has admin access
            if (user.Permissions.HasPermission(Permissions.Administrator))
            {
                // Creats a new embed
                var em = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Azure,
                    Title = $"Recent Requests",
                    Description = $"Requests made in the last {searchTime} days.."
                };

                //Loop Through requests and check the time frame
                foreach (var date in Xilften.requests)
                {
                    DateTime dateTimeToCheck = date.Key;

                    TimeSpan difference = DateTime.Now - dateTimeToCheck;

                    int index = 0;

                    
                    //Swwitch Case to manage the time frame option chose.
                        switch (searchTime)
                        {
                            case 0:
                                index ++;

                                em.AddField(date.Value.dateMade.ToString(), $"{date.Value.memberName} Requested {date.Value.request} with Added response of {date.Value.wasAdded} ", false);
                                break;

                            case 7:
                                if (difference.TotalDays <= 7)
                                {
                                em.AddField(date.Value.dateMade.ToString(), $"{date.Value.memberName} Requested {date.Value.request} with Added response of {date.Value.wasAdded} ", false);
                                index++;
                                }
                                break;

                            case 30:
                                if (difference.TotalDays <= 30)
                                {
                                em.AddField(date.Value.dateMade.ToString(), $"{date.Value.memberName} Requested {date.Value.request} with Added response of {date.Value.wasAdded} ", false);
                                index++;
                                }
                                break;
                        }                    
                }

                //Build the embed
                em.Build();

                // Send the embed to the user in a Private Message
                await user.SendMessageAsync(embed: em);
            }
            else  //If the user does not have admin access
            { await user.SendMessageAsync($"You do not have the Authority to access this command. This strike has been noted.  Continued use of this command will result in your Automatic ban from the server.");}

        }

        //Remove the recent requests with the time frame(7 days,30 days, all time)
        [SlashCommand("RemoveRecent", "Remove the recent requests!")]
        public async Task ResetRecent(InteractionContext ctx,
            [Choice("7 days", 7)]
            [Choice("30 days", 30)]
            [Choice("all time", 0)]
            [Option("clearTime", "Number of days to clear requests")] long clearTime = 0)
        {
            var user = ctx.Member;

            //Check if the user has admin access
            if (user.Permissions.HasPermission(Permissions.Administrator))
            {


                // Creats a new embed
                var em = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Azure,
                    Title = $"Clear Request Database",
                    Description = $"Clearing the last {clearTime} days.(0 = All)"
                };
                int index = 0;

                //Loop Through requests and check the time frame
                foreach (var date in Xilften.requests)
                {
                    DateTime dateTimeToCheck = date.Key;

                    TimeSpan difference = DateTime.Now - dateTimeToCheck;

                    //Sitch Case to manage the time frame option chose.
                    switch (clearTime)
                    {
                        case 0:
                            index = Xilften.requests.Count;
                            Xilften.requests.Clear();
                            break;

                        case 7:
                            if (difference.TotalDays <= 7)
                            {
                                Xilften.requests.Remove(date.Key);
                                index++;
                            }
                            break;

                        case 30:
                            if (difference.TotalDays <= 30)
                            {
                                Xilften.requests.Remove(date.Key);
                                index++;
                            }
                            break;
                    }
                }

                // Build the embed
                em.AddField("Requests Cleared!", $"{index} Requests Successfully Removed.", false);
                em.Build();

                // Send the embed to the user in a Private Message
                await user.SendMessageAsync(embed: em);
            }
            else //If the user does not have admin access
            { await user.SendMessageAsync($"You do not have the Authority to access this command. This strike has been noted.  Continued use of this command will result in your Automatic ban from the server."); }

        }

        //Gets the requests for a user with the time frame(7 days,30 days, all time)
        //[SlashCommand("UserRequests", "Get Requests for a user in a time frame!")]
        //public async Task UserRequests(InteractionContext ctx, [Option("user", "User to search Request history")] DiscordUser  member,
        //    [Choice("7 days", 7)]
        //    [Choice("30 days", 30)]
        //    [Choice("all time", 0)]
        //    [Option("searchTime", "Number of days to search requests")] long searchTime = 0)
        //{
        //    var user = ctx.Member;

        //    // Check if the user has admin access
        //    if (user.Permissions.HasPermission(Permissions.Administrator))
        //    {
        //        var em = new DiscordEmbedBuilder
        //        {
        //            Color = DiscordColor.Azure,
        //            Title = $"Requests Search",
        //            Description = $"Requests for User {member.Username}."
        //        };

        //        int index = 0;

        //        //Loop Through requests and check the time frame
        //        foreach (var date in Xilften.requests)
        //        {
        //            if (date.Value.memberName == member.Username)
        //            {
        //                DateTime dateTimeToCheck = date.Key;

        //                TimeSpan difference = DateTime.Now - dateTimeToCheck;


        //                //Swwitch Case to manage the time frame option chose.
        //                switch (searchTime)
        //                {
        //                    case 0:
        //                        em.AddField($"{date.Value.dateMade}:", $"{date.Value.request} , Added:{date.Value.wasAdded} ", false);
        //                        index++;
        //                        break;

        //                    case 7:
        //                        if (difference.TotalDays <= 7)
        //                        {
        //                            em.AddField($"{date.Value.dateMade}:", $"{date.Value.request} , Added:{date.Value.wasAdded} ", false);
        //                            index++;
        //                        }
        //                        break;

        //                    case 30:
        //                        if (difference.TotalDays <= 30)
        //                        {
        //                            em.AddField($"{date.Value.dateMade}:", $"{date.Value.request} , Added:{date.Value.wasAdded} ", false);
        //                            index++;
        //                        }
        //                        break;
        //                }
        //            }
        //        }

        //        //Build the embed
        //        em.Build();

        //        // Send the embed to the user in a Private Message
        //        await user.SendMessageAsync(embed: em);
        //    }
        //    else //If the user does not have admin access
        //    { await user.SendMessageAsync($"You do not have the Authority to access this command. This strike has been noted.  Continued use of this command will result in your Automatic ban from the server."); }

        //}
    }
}
