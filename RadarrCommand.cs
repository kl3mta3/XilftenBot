using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using DSharpPlus.Interactivity.Extensions;
using System.Text;



namespace XilftenBot
{
    /// <summary>
    /// Application command for Radarr
    /// </summary>
    public class RadarrCommand : ApplicationCommandModule
    {

        //Application Command for Adding Movies to Radarr
        [SlashCommand("Movie", "Search for and add Movie!")]
        
        public async Task MovieCommand(InteractionContext ctx, [Option("Movie", "Name of Movie")] string movieName)
        {
            //Check if Radarr is Activated.
            if (Xilften.radarrActivated)
            { 
                //Creates a Delayed Response to the Interaction.(Allows us to Respond to the Interaction later by editing the response.)
                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                DiscordMember member = ctx.Member;
                Console.WriteLine($"Movie Search Recieved:{movieName} by User {member.Username}");


                //search to see if movie exists.
                if (movieName != null)
                {
                    try
                    {
                        // Create a new HttpClient and Radarr API request.
                        using (var httpClient = new HttpClient())
                        {
                            // Add API key to request headers
                            httpClient.DefaultRequestHeaders.Add("X-Api-Key", Xilften.radarrAPI);

                            string requestUrl = Xilften.radarrIP + $"api/v3/movie/lookup?term={movieName}";

                            // Send GET request to Radarr
                            var response = httpClient.GetAsync(requestUrl).Result;
                            var json = httpClient.GetAsync(requestUrl).Result.Content.ReadAsStringAsync().Result;

                            // Check if request was successful
                            if (response.IsSuccessStatusCode)
                            {


                                string jsonContent = json.ToString();

                                //Var to hold the JSON response to build Request.(Do not Modify)
                                string path = "";
                                string imdbId = "";
                                string status = "";
                                bool searchForMovie = true;
                                string monitor = "movieOnly";
                                bool monitored = true;

                                // Deserialize JSON response (many Movies) into an Array.
                                JArray objArray = JsonConvert.DeserializeObject<JArray>(json);

                                //Gets the first Movie in the Array and converts it to a json string.
                                var getMovieObj = objArray[0];
                                var getMovieString = getMovieObj.ToString();

                                //Use Try Catch to get the values from the JSON so empty doesnt Crash bot.
                                try { imdbId = getMovieObj["imdbId"].ToString(); } catch { }
                                try { path = getMovieObj["path"].ToString(); } catch { }
                                try { status = getMovieObj["status"].ToString(); } catch { }

                                // Deserialize JSON String into a Movie Object.
                                var movieDeserializedGet = JsonConvert.DeserializeObject<Movie>(getMovieString);

                                //Create a new Request Object.
                                var newRequest = new Request
                                {
                                    memberName = member.Nickname,
                                    request = movieName,
                                    dateMade = DateTime.Now,

                                };

                                if (path != "") //If we do not have a path then the movie is not on the server, Check to see if we should add it and add it if we should.
                                {

                                    //Create an Embed to send
                                    var em = new DiscordEmbedBuilder
                                    {
                                        Color = DiscordColor.SpringGreen,
                                        Title = $"{movieDeserializedGet.title}",
                                        //Url = "https://www.imdb.com/title/" + imdbId,
                                        Description = $"Movie is already on the server."
                                    };

                                    //If we have an IMDB ID, add it to the Embed.
                                    if (imdbId != "") 
                                    {
                                        em.WithUrl("https://www.imdb.com/title/" + imdbId);

                                    }
                                    em.AddField("Added on: ", $"{movieDeserializedGet.added}", true);
                                    em.AddField("Status ", $"{status}", true);
                                    em.AddField("IMDB Id: ", $"{movieDeserializedGet.imdbId}", true);
                                    em.AddField("Release year: ", $"{movieDeserializedGet.year}", true);
                                    em.AddField("Overview: ", $"{movieDeserializedGet.overview}", false);
                                    em.Build();

                                    //Create the Webhook and add the Embed.
                                    var wh = new DiscordWebhookBuilder();
                                    wh.AddEmbed(em);
                                    await ctx.EditResponseAsync(wh);

                                    //Update if Request was added, and add it to the Requests List.
                                    newRequest.wasAdded = false;
                                    Xilften.WriteToLog($"{member.Username} requested movie {movieDeserializedGet.title} it Existed on the Server.");
                                    Xilften.requests.Add(DateTime.Now, newRequest);

                                }
                                else if (path == "")
                                {

                                    // Building Embed
                                    var em = new DiscordEmbedBuilder
                                    {
                                        Color = DiscordColor.Red,
                                        Title = $"{movieDeserializedGet.title}",
                                        Url = "https://www.imdb.com/title/" + imdbId,
                                        Description = $"This movie is not on the server."
                                    };
                                    if (imdbId != "")
                                    {
                                        em.WithUrl("https://www.imdb.com/title/" + imdbId);

                                    }

                                    var rootfolder= Xilften.radarrRootPath;
                                    

                                    //Choose the Emoji that will be used to react to the Embed
                                    var emoji = DiscordEmoji.FromName(ctx.Client, ":ok_hand:");

                                    em.AddField("IMDB Id: ", $"{movieDeserializedGet.imdbId}", true);
                                    em.AddField("Status ", $"{status}", true);
                                    em.AddField("Release year: ", $"{movieDeserializedGet.year}", true);
                                    em.AddField("Overview: ", $"{movieDeserializedGet.overview}", false);

                                    //Creates a Blank Field to make the Embed look better
                                    em.AddField("\u200b", "\u200b");

                                    em.AddField($"{ctx.Member.Username},", $"React with {emoji} within 30 seconds to add {movieDeserializedGet.title} to the server!", false);
                                    em.Build();



                                    //adding Embed to Webhook
                                    var wh = new DiscordWebhookBuilder();
                                    wh.AddEmbed(em);

                                    //Update the Interaction Response with the Embed
                                    var message = await ctx.EditResponseAsync(wh);

                                    //waiting for reaction
                                    var result = await message.WaitForReactionAsync(ctx.Member, emoji);

                                    //if reacted to in time Send Add Post to Server
                                    if (!result.TimedOut)
                                    {


                                        string postUrl = Xilften.radarrIP + $"api/v3/movie";

                                        movieDeserializedGet.rootFolderPath = rootfolder;
                                        movieDeserializedGet.qualityProfileId = 1;
                                        movieDeserializedGet.SetAddOptions(searchForMovie, monitor);

                                        string updatedPost = JsonConvert.SerializeObject(movieDeserializedGet, Formatting.Indented);
                                        var payload = new StringContent(updatedPost, Encoding.UTF8, "application/json");
                                        var postResponse = httpClient.PostAsync(postUrl, payload).Result.Content.ReadAsStringAsync();
                                        var postResults = postResponse.Result;

                                        //Here we check to see if post returned successfully
                                        if (postResponse.IsCompletedSuccessfully)
                                        {
                                            Console.WriteLine($"The movie {movieDeserializedGet.title} was added to Radarr.");

                                            await ctx.Channel.SendMessageAsync($" The movie {movieDeserializedGet.title} is adding to the server. Allow 30 min to 1 hour");

                                            newRequest.wasAdded = true;
                                            Xilften.WriteToLog($"{member.Username} requested movie {movieDeserializedGet.title} it Existed on the Server.");
                                            Xilften.requests.Add(DateTime.Now, newRequest);
                                        }
                                        else //If not successful send error message
                                        {
                                            Console.WriteLine("Post sent and response Recieved Error. ");

                                            await ctx.Channel.SendMessageAsync("There was an error adding the movie");

                                            newRequest.wasAdded = false;
                                            Xilften.WriteToLog($"{member.Username} requested movie {movieDeserializedGet.title}, but there was an error. The Post did not complete Successfully.");
                                            Xilften.requests.Add(DateTime.Now, newRequest);

                                        }
                                    }

                                }
                            }
                        }
                    }
                    catch(Exception ex)// There is no connection to Radarr Send Error Message.
                    {
                        Console.WriteLine($"Post sent and response Recieved Error.{ex.Message} ");
                        await ctx.Channel.SendMessageAsync("There was an error adding the show");
                        Xilften.WriteToLog($"{ctx.Member.Username} requested show {movieName} but there was an Error.  Error Message : {ex.Message}");
                    }
                }
            }
            else //Radarr Is not active on this server send error message.(Likely Missing the Radarr API Key or Radarr IP)
            {
                await ctx.Channel.SendMessageAsync("Radarr is not Activated on this Server.");
                Xilften.WriteToLog($"{ctx.Member.Username} requested show {movieName} but Radarr is not activated on this Server.");
            }
        }

        [SlashCommand("RestartRadarr", "Restart the Radarr Server!")]
        public async Task RestartSonarrCommand(InteractionContext ctx)
        {
            DiscordMember user = ctx.Member;
            DiscordChannel channel = ctx.Channel;

            if (user.Permissions.HasPermission(Permissions.Administrator))
            {
                using (var httpClient = new HttpClient())
                {
                    // Add API key to request headers
                    httpClient.DefaultRequestHeaders.Add("X-Api-Key", Xilften.radarrAPI);


                    // Send Post request to Radarr
                    string requestUrl = Xilften.radarrIP + $"api/v3/system/restart";
                    var response = httpClient.GetAsync(requestUrl).Result;
                    var json = httpClient.GetAsync(requestUrl).Result.Content.ReadAsStringAsync().Result;

                    // Check if request was successful
                    if (response.IsSuccessStatusCode)
                    {

                        var em = new DiscordEmbedBuilder
                        {
                            Color = DiscordColor.Azure,
                            Title = $"Radarr Reseting",
                            Description = $"Please Allow a Few Minutes."
                        };

                        em.Build();

                        await channel.SendMessageAsync(embed: em);

                    }
                    else
                    {
                        await channel.SendMessageAsync("Error Reseting RAdarr");
                    }
                }
            }
        }



    }
}

