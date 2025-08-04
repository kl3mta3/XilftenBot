using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using static System.Net.WebRequestMethods;
using System.Threading;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity.Extensions;
using XilftenBot;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Headers;

namespace XilftenBot
{
    /// <summary>
    /// Application command for Sonarr
    /// </summary>
    public class SonarrCommand : ApplicationCommandModule
    {
        //Application Command for Adding TV Shows to Sonarr
        [SlashCommand("TV", "Search for and add TV show!")]
        public async Task TVCommand(InteractionContext ctx, [Option("Tv", "Name of Show")] string showName)
        {
            //Check if Sonarr is Activated.
            if (Xilften.sonarrActivated)
            {
                //Creates a Delayed Response to the Interaction. (Allows us to Respond to the Interaction later by editing the response.)
                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                DiscordChannel channel = ctx.Channel;
                DiscordMember member = ctx.Member;

                Console.WriteLine($"TV Search Recieved:{showName} by User {member.Username}");


                //search to see if show exists.
                if (showName != null)
                {
                    try
                    {
                        // Create a new HttpClient and Sonarr API request.
                        using (var httpClient = new HttpClient())
                        {
                            // Add API key to request headers.
                            httpClient.DefaultRequestHeaders.Add("X-Api-Key", Xilften.sonarrAPI);

                            // Send GET request to Sonarr.
                            string requestUrl = Xilften.sonarrIP + $"api/v3/series/lookup?term={showName}";
                            var response = httpClient.GetAsync(requestUrl).Result;
                            var json = httpClient.GetAsync(requestUrl).Result.Content.ReadAsStringAsync().Result;

                            // Check if request was successful.
                            if (response.IsSuccessStatusCode)
                            {
                                //Var to hold the JSON response to build Request.(Do not Modify)
                                string jsonContent = json.ToString();
                                string path = "";
                                string imdbId = "";
                                string monitor = "";
                                int _qualityProfileId = 1;
                                bool searchForMissingEpisodes = true;
                                bool searchForCutoffUnmetEpisodes = false;

                                // Deserialize JSON response (many Shows) into an Array.
                                JArray objArray = JsonConvert.DeserializeObject<JArray>(json);

                                //Gets the first Show in the Array and converts it to a json string.
                                var getSeriesObj = objArray[0];
                                var getSeriesString = getSeriesObj.ToString();

                                //Use Try Catch to get the values from the JSON so empty doesnt Crash bot.
                                try { imdbId = getSeriesObj["imdbId"].ToString(); } catch { }
                                try { path = getSeriesObj["path"].ToString(); } catch { }
                                try { path = getSeriesObj["qualityProfileId"].ToString(); } catch { }
                                try { monitor = getSeriesObj["monitor"].ToString(); } catch { }



                                // Deserialize JSON String into a Series Object.
                                var seriesDeserializedGet = JsonConvert.DeserializeObject<Series>(getSeriesString);

                                

                                //Create a new Request Object.
                                var newRequest = new Request
                                {
                                    memberName = member.Username,
                                    request=showName,
                                    dateMade=DateTime.Now,
                                    title=seriesDeserializedGet.title,
                                    

                                };

                                //If we have a path, the show is already on the server.
                                Console.WriteLine("path= " + path);
                                string _path = "X:";
                                //if (path != "")
                                if (path.Contains("X:"))
                                    {
                                    Console.WriteLine("Is on server Triggered");
                                    //Create an Embed to send.
                                    var em = new DiscordEmbedBuilder
                                    {
                                        Color = DiscordColor.SpringGreen,
                                        Title = $"{seriesDeserializedGet.title}",
                                        Url = "https://www.imdb.com/title/" + imdbId,
                                        Description = $"Show is already on the server."
                                    };

                                    //If we have an IMDB ID, add it to the Embed.
                                    if (imdbId != "")
                                    {
                                        em.WithUrl("https://www.imdb.com/title/" + imdbId);
                                    }

                                    em.AddField("Added on: ", $"{seriesDeserializedGet.added}", true);
                                    em.AddField("Status: ", $"{seriesDeserializedGet.status}", true);
                                    em.AddField("Network: ", $"{seriesDeserializedGet.network}", true);
                                    em.AddField("First Aired: ", $"{seriesDeserializedGet.firstAired}", true);
                                    // cut overview to fit incase its too long.
                                    string _overview = seriesDeserializedGet.overview.Length > 1000
                                        ? seriesDeserializedGet.overview.Substring(0, 1000)
                                        : seriesDeserializedGet.overview;

                                    em.AddField("Overview: ", $"{_overview}", false);
                                    em.Build();

                                    //Create the Webhook and add the Embed.
                                    var wh = new DiscordWebhookBuilder();
                                    wh.AddEmbed(em);

                                    //Update if Request was added, and add it to the Requests List.
                                    newRequest.wasAdded = false;
                                    Xilften.WriteToLog($"{member.Username} requested show {seriesDeserializedGet.title} it Existed on the Server.");
                                    Xilften.requests.Add(DateTime.Now, newRequest);

                                    //Update the Interaction Response with the Embed
                                    await ctx.EditResponseAsync(wh);
                                }
                                else  
                                {
                                    Console.WriteLine("NOT on server Triggered");
                                    // Building Embed
                                    var em = new DiscordEmbedBuilder
                                    {
                                        Color = DiscordColor.Red,
                                        Title = $"{seriesDeserializedGet.title}",
                                        Url = "https://www.imdb.com/title/" + imdbId,
                                        Description = $"This show is not on the server."
                                    };
                                    if (imdbId != "")
                                    {
                                        em.WithUrl("https://www.imdb.com/title/" + imdbId);

                                    }

                                    //Choose the Emoji that will be used to react to the Embed
                                    var emoji = DiscordEmoji.FromName(ctx.Client, ":ok_hand:");

                                    em.AddField("Status: ", $"{seriesDeserializedGet.status}", true);
                                    em.AddField("Network: ", $"{seriesDeserializedGet.network}", true);
                                    em.AddField("First Aired: ", $"{seriesDeserializedGet.firstAired}", true);
                                    // cut overview to fit incase its too long.
                                    string _overview = seriesDeserializedGet.overview.Length > 1000
                                        ? seriesDeserializedGet.overview.Substring(0, 1000)
                                        : seriesDeserializedGet.overview;

                                    em.AddField("Overview: ", $"{_overview}", false);

                                    ////Creates a Blank Field to make the Embed look better
                                    //em.AddField("\u200b", "\u200b");

                                    

                                    em.AddField($"{member.Username},", $"React with {emoji} within 30 seconds to add {seriesDeserializedGet.title} to the server!", false);
                                    em.Build();


                                    //adding Embed to Webhook
                                    var wh = new DiscordWebhookBuilder();
                                    wh.AddEmbed(em);

                                    //Editing the Response
                                    var message = await ctx.EditResponseAsync(wh);

                                    //waiting for reaction
                                    var result = await message.WaitForReactionAsync(ctx.Member, emoji);



                                    //if reacted to in time Send Add Post to Server
                                    if (!result.TimedOut)
                                    {


                                        string postUrl = Xilften.sonarrIP + $"api/v3/series";

                                        seriesDeserializedGet.rootFolderPath = Xilften.sonarrRootPath;
                                        seriesDeserializedGet.qualityProfileId = 1;
                                        seriesDeserializedGet.SetAddOptions(searchForMissingEpisodes, monitor, searchForCutoffUnmetEpisodes);

                                        string updatedPost = JsonConvert.SerializeObject(seriesDeserializedGet, Formatting.Indented);
                                        var payload = new StringContent(updatedPost, Encoding.UTF8, "application/json");

                                        // Clear and set correct headers
                                        httpClient.DefaultRequestHeaders.Accept.Clear();
                                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                        // Make the POST request the *right* way
                                        HttpResponseMessage response1 = await httpClient.PostAsync(postUrl, payload);

                                        // Read the response content
                                        string postResults = await response1.Content.ReadAsStringAsync();



                                        //Here we check to see if post returned successfully
                                        if (response1.IsSuccessStatusCode)
                                        {
                                            Console.WriteLine("Post sent and response Received Successfully. ");


                                            var ef = new DiscordEmbedBuilder
                                            {
                                                Color = DiscordColor.SpringGreen,
                                                Title = $"{seriesDeserializedGet.title}",
                                                Url = "https://www.imdb.com/title/" + imdbId,
                                                Description = $"Show was added to server."
                                            };


                                            ef.AddField("Note:", $"Allow 1 hour per season for it to update.", true);
                                            ef.Build();

                                            var hh = new DiscordWebhookBuilder();
                                            hh.AddEmbed(ef);



                                            //await ctx.Channel.SendMessageAsync($"The show {seriesDeserializedGet.title} is adding to the server. Allow 1 hour per season for it to update");
                                            await ctx.Channel.SendMessageAsync(ef);
                                            newRequest.wasAdded = true;
                                            Xilften.WriteToLog($"{member.Username} requested show {seriesDeserializedGet.title} it Existed on the Server.");

                                            Xilften.requests.Add(DateTime.Now, newRequest);
                                            Console.WriteLine(Xilften.requests.ToString());


                                        }
                                        else   //If not successful send error message
                                        {
                                            Console.WriteLine("Post sent and response Received Error. ");
                                            Console.WriteLine($"Status Code: {response1.StatusCode}");
                                            Console.WriteLine($"Raw Response:\n{postResults}");
                                            await ctx.Channel.SendMessageAsync("There was an error adding the show");

                                            newRequest.wasAdded = false;
                                            Xilften.WriteToLog($"{member.Username} requested show {seriesDeserializedGet.title}, but there was an error. The Post did not complete Successfully.");

                                            Xilften.requests.Add(DateTime.Now, newRequest);

                                        }
                                    }

                                }

                            }
                            else //The post failed to send send error message.
                            {
                                Console.WriteLine("Failed to ping Sonarr. Status code: " + response.StatusCode);

                                await ctx.Channel.SendMessageAsync("There was an error adding the show");

                                Xilften.WriteToLog($"{member.Username} requested show {showName} but the Post Results were Unsuccessful.");

                            }
                        }



                    }
                    catch (Exception ex)// There is no connection to Sonarr Send Error Message.
                    {
                        Console.WriteLine($"Error Message: {ex.Message}");

                        await ctx.Channel.SendMessageAsync("There was an error adding the show");

                        Xilften.WriteToLog($"{member.Username} requested show {showName} But it Failed to establish a connection to Sonarr. Error message: {ex.Message}.");
                    }

                }

            }
            else //Sonarr Is not active on this server send error message.(Likely Missing the Sonarr API Key or Sonarr IP)
            {
                await ctx.Channel.SendMessageAsync("Sonarr is not active on this Server.");
                Xilften.WriteToLog($"{ctx.Member.Username} requested show {showName} but Sonarr is not activated on this Server.");

            }

        }

        //Command to Restart Sonarr
        [SlashCommand("RestartSonarr", "Restart the Sonarr Server!")]
        public async Task RestartSonarrCommand(InteractionContext ctx)
        {
            DiscordMember user = ctx.Member;
            DiscordChannel channel = ctx.Channel;

            // Check if user has permission to use this command
            if (user.Permissions.HasPermission(Permissions.Administrator))
            {
                // Send request to Sonarr to restart
                using (var httpClient = new HttpClient())
                {
                    // Add API key to request headers
                    httpClient.DefaultRequestHeaders.Add("X-Api-Key", Xilften.sonarrAPI);


                    // Send Post request to Sonarr
                    string requestUrl = Xilften.sonarrIP + $"api/v3/system/restart";
                    var payload = new StringContent("", Encoding.UTF8, "application/json");
                    var response = httpClient.PostAsync(requestUrl, payload).Result;

                    // Check if request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        //Create the and build the Embed
                        var em = new DiscordEmbedBuilder
                        {
                            Color = DiscordColor.Azure,
                            Title = $"Sonarr Reseting",
                            Description = $"Please Allow a Few Minutes."
                        };
                        em.Build();

                        //Send the Embed in a private message
                        await channel.SendMessageAsync(embed: em);
                    }
                    else
                    {
                        //If request Unsuccessful send error message
                        await channel.SendMessageAsync("Error Reseting Sonarr");
                    }
                }
            }
        }

    }
}


