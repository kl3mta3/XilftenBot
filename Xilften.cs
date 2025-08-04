using System.Configuration;
using DSharpPlus;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using System.Text.Json;




namespace XilftenBot
 {
    
    class Xilften
    {

        //Sonarr Var
        public static string sonarrIP = ConfigurationManager.AppSettings.Get("SonarrIP");
        public static string sonarrAPI = ConfigurationManager.AppSettings.Get("SonarrAPIKey");

        public static string sonarrRootPath ="";
        public  static  bool sonarrActivated=false;

        //Radarr var
        public static string radarrIP = ConfigurationManager.AppSettings.Get("RadarrIP");
        public static string radarrAPI = ConfigurationManager.AppSettings.Get("RadarrAPIKey");

        public static string radarrRootPath = "";
        public static bool radarrActivated = false;

        //Discord var
        public static string discordBotToken = ConfigurationManager.AppSettings.Get("Token");
        

        //Discord Guild Id
        public static ulong guildID = ulong.Parse(ConfigurationManager.AppSettings.Get("GuildId"));
        

        // local appdata path
        public static string appDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string xilftenbotFolderPath = Path.Combine(appDataFolderPath, "Xilftenbot");

        // log var
        public static string logFilePath = Path.Combine(xilftenbotFolderPath, "log.txt");

        //request database var
        public static string requestsFilePath = Path.Combine(xilftenbotFolderPath, "request.json");
        public static Dictionary<DateTime, Request > requests = new Dictionary<DateTime, Request>();


        //Programs Main Loop
        static async Task Main(string[] args)
        {

            //Create Directory if it doesnt exist.

            if (!Directory.Exists(xilftenbotFolderPath))
            {
                Directory.CreateDirectory(xilftenbotFolderPath);
                logFilePath = Path.Combine(xilftenbotFolderPath, "log.txt");
                requestsFilePath = Path.Combine(xilftenbotFolderPath, "request.json");
            }

           
            //Load requests if exists
            if (File.Exists(requestsFilePath))
            {
                requests = LoadDictionaryFromJson(requestsFilePath);

            }
            // Subscribe to application exit event
            AppDomain.CurrentDomain.ProcessExit += ProcessExitHandler;


            // Subscribe to unhandled exception event
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            // Create a DiscordClient
            var discord = new DiscordClient(new DiscordConfiguration()
                {
                    Token = discordBotToken,
                    TokenType = TokenType.Bot,
                    Intents = DiscordIntents.AllUnprivileged | DiscordIntents.All,
                    MinimumLogLevel = LogLevel.Information
                });


            //Check if Discord Token is set
            if (!string.IsNullOrEmpty(discordBotToken))
            {

                await discord.ConnectAsync();

                discord.UseInteractivity(new InteractivityConfiguration()
                {
                    PollBehaviour = PollBehaviour.KeepEmojis,
                    Timeout = TimeSpan.FromSeconds(30)
                });
               
                var slash = discord.UseSlashCommands();
                slash.RegisterCommands<RadarrCommand>(guildID);
                slash.RegisterCommands<BotCommand>(guildID);
                slash.RegisterCommands<SonarrCommand>(guildID);

               
            }
            else
            {
                Console.WriteLine("Please add your bots Discord Token in the App.config file.");
            }

            //Check if Sonarr API or IP is set
            if (!string.IsNullOrEmpty(sonarrAPI) || !string.IsNullOrEmpty(sonarrIP))
            {
                
              

                SetSonarrRootPath();
                sonarrActivated = true;
            }
            else
            {
                if(string.IsNullOrEmpty(sonarrAPI) && string.IsNullOrEmpty(sonarrIP))
                {
                    Console.WriteLine("Please add your SonarrAPI and SonarrIP in the App.config file.");
                    sonarrActivated = false;
                }
                else if(!string.IsNullOrEmpty(sonarrAPI) && string.IsNullOrEmpty(sonarrIP))
                {
                    Console.WriteLine("You are missing your SonarrIP in the App.config file.");
                    sonarrActivated = false;
                }
                else if (string.IsNullOrEmpty(sonarrAPI) && !string.IsNullOrEmpty(sonarrIP))
                {
                    Console.WriteLine("You are missing your SonarrAPI in the App.config file.");
                    sonarrActivated = false;
                }
            }

            //Check if Radarr API or IP is set
            if (!string.IsNullOrEmpty(radarrAPI) || !string.IsNullOrEmpty(radarrIP))
            {


                SetRadarrRootPath();
                radarrActivated = true;
               

            }
            else
            {
                if (string.IsNullOrEmpty(radarrAPI) && string.IsNullOrEmpty(radarrIP))
                {
                    Console.WriteLine("Please add your RadarrAPI and RadarrIP in the App.config file.");
                    radarrActivated = false;
                }
                else if (!string.IsNullOrEmpty(radarrAPI) && string.IsNullOrEmpty(radarrIP))
                {
                    Console.WriteLine("You are missing your RadarrIP in the App.config file.");
                    radarrActivated = false;
                }
                else if (string.IsNullOrEmpty(radarrAPI) && !string.IsNullOrEmpty(radarrIP))
                {
                    Console.WriteLine("You are missing your RadarrAPI in the App.config file.");
                    radarrActivated = false;
                }
            }

            Console.Clear();
            Console.WriteLine("XilftenBot is ready and waiting for requests.");
            await WriteToLog("XilftenBot was started.");

            //Updats the bots status every 10 minutes with a random status message
            await UpdateStatus(discord);
            var timer = new PeriodicTimer(TimeSpan.FromMinutes(5));
            while (await timer.WaitForNextTickAsync())
            {
                UpdateStatus(discord);
            }

    

            //Keep it running
            await Task.Delay(-1);
        }
     
        //Method to Update the bots status
        public static async Task UpdateStatus(DiscordClient discord)
        {
            var newStatus = GetRandomStatus();
            await discord.UpdateStatusAsync(new DiscordActivity(newStatus, ActivityType.Watching));

        }

        // Generats a random status message
        public static string GetRandomStatus()
        {
              Random random = new Random();

            var statusMessages = new List<string>()
            {
                "Game of Thrones",
                "Friends",
                "Breaking Bad",
                "Stranger Things",
                "The Office (US)",
                "The Mandalorian",
                "The Crown",
                "The Witcher",
                "The Simpsons",
                "Rick and Morty",
                "The Big Bang Theory",
                "Brooklyn Nine-Nine",
                "Narcos",
                "Peaky Blinders",
                "The Walking Dead",
                "Westworld",
                "Black Mirror",
                "Vikings",
                "The Haunting of Hill House",
                "Chernobyl",
                "Sherlock",
                "The Marvelous Mrs. Maisel",
                "Money Heist",
                "The Handmaid's Tale",
                "Stranger Things",
                "Lucifer",
                "Ozark",
                "Dark",
                "Avatar: The Last Airbender",
                "Cobra Kai",
                "The Crown",
                "The Umbrella Academy",
                "Mindhunter",
                "Better Call Saul",
                "The Boys",
                "The Good Place",
                "Doctor Who",
                "Dexter",
                "The X-Files",
                "Fargo",
                "Suits",
                "FRIENDS",
                "The Walking Dead",
                "Prison Break",
                "Supernatural",
                "The 100",
                "Grey's Anatomy",
                "The Flash",
                "Arrow",
                "Stranger Things",
                "The Office",
                "Breaking Bad",
                "How I Met Your Mother",
                "Modern Family",
                "Gossip Girl",
                "The Vampire Diaries",
                "Glee",
                "The Simpsons",
                "South Park",
                "Rick and Morty",
                "Family Guy",
                "Big Bang Theory",
                "Friends",
                "Game of Thrones",
                "Stranger Things",
                "The Walking Dead",
                "Breaking Bad",
                "The Office",
                "Sherlock",
                "The Mandalorian",
                "Narcos",
                "Fargo",
                "Westworld",
                "Peaky Blinders",
                "Mindhunter",
                "Black Mirror",
                "True Detective",
                "The Crown",
                "Better Call Saul",
                "Dexter",
                "The Witcher",
                "The Haunting of Hill House",
                "Game of Thrones",
                "Breaking Bad",
                "Stranger Things",
                "Sherlock",
                "The Office",
                "Friends",
                "The Crown",
                "The Mandalorian",
                "Narcos",
                "Peaky Blinders",
                "Mindhunter",
                "Westworld",
                "Black Mirror",
                "The Witcher",
                "Better Call Saul",
                "The Haunting of Hill House",
                "Money Heist",
                "The Boys",
                "The Umbrella Academy",
                "Game of Thrones",
                "The Shawshank Redemption",
                "The Godfather",
                "The Dark Knight",
                "Pulp Fiction",
                "Forrest Gump",
                "Inception",
                "Fight Club",
                "The Matrix",
                "Goodfellas",
                "The Lord of the Rings: The Fellowship of the Ring",
                "Star Wars: A New Hope",
                "Avengers: Endgame",
                "Titanic",
                "Interstellar",
                "The Silence of the Lambs",
                "Schindler's List",
                "The Lord of the Rings: The Return of the King",
                "Jurassic Park",
                "The Lion King",
                "Gladiator",
                "The Departed",
                "The Green Mile",
                "Back to the Future",
                "The Prestige",
                "Saving Private Ryan",
                "The Godfather: Part II",
                "The Avengers",
                "The Social Network",
                "The Wolf of Wall Street",
                "Harry Potter and the Philosopher's Stone",
                "The Lord of the Rings: The Two Towers",
                "The Godfather: Part III",
                "The Dark Knight Rises",
                "The Usual Suspects",
                "The Shawshank Redemption",
                "The Empire Strikes Back",
                "Forrest Gump",
                "Schindler's List",
                "The Lord of the Rings: The Return of the King",
                "One Flew Over the Cuckoo's Nest",
                "The Matrix",
                "Goodfellas",
                "Se7en",
                "Life Is Beautiful",
                "The Silence of the Lambs",
                "It's a Wonderful Life",
                "Star Wars",
                "Saving Private Ryan",
                "The Green Mile",
                "The Pianist",
                "Gladiator",
                "The Departed",
                "The Lion King",
                "Back to the Future",
                "The Prestige",
                "The Shining",
                "Django Unchained",
                "WALL-E",
                "The Truman Show",
                "The Grand Budapest Hotel",
                "The Terminator",
                "Jurassic Park",
                "The Silence of the Lambs",
                "Die Hard",
                "The Godfather: Part II",
                "Good Will Hunting",
                "The Big Lebowski",
                "Apocalypse Now",
                "Toy Story",
                "Braveheart",
                "The Shawshank Redemption",
                "The Empire Strikes Back",
                "The Lord of the Rings: The Two Towers",
                "The Godfather: Part III",
                "The Dark Knight Rises",
                "The Usual Suspects",
                "The Lord of the Rings: The Fellowship of the Ring",
                "The Godfather",
                "The Silence of the Lambs",
                "Pulp Fiction",
                "Forrest Gump",
                "The Matrix",
                "Goodfellas",
                "The Green Mile",
                "Schindler's List",
                "The Lion King",
                "Gladiator",
                "Back to the Future",
                "The Prestige",
                "The Departed",
                "Saving Private Ryan",
                "The Avengers",
                "The Social Network",
                "The Wolf of Wall Street",
                "Harry Potter and the Philosopher's Stone",
                "Inception",
                "The Lord of the Rings: The Return of the King",
                "Jurassic Park",
                "Interstellar",
                "Titanic",
                "The Dark Knight"

            };
            int index = random.Next(statusMessages.Count);
            return statusMessages[index];
        }

        //This method is used to get the root path of the Sonarr server
        public static void SetSonarrRootPath()
        {
            using (var client = new HttpClient())
            {
               
                client.DefaultRequestHeaders.Add("X-Api-Key", Xilften.sonarrAPI);

                string rootFloderPath = "";
                string requestUrl = sonarrIP + $"api/v3/rootfolder";

                var response = client.GetAsync(requestUrl).Result;
                var json = client.GetAsync(requestUrl).Result.Content.ReadAsStringAsync().Result;

                JArray objArray = JsonConvert.DeserializeObject<JArray>(json);
                string path = "";
                if (response.IsSuccessStatusCode)
                {
                    
                    foreach (var obj in objArray)
                    {
                        if (obj != null)
                        {
                            try { path = obj["path"].ToString(); } catch { }
                            if (path != "")
                            {
                                break;
                            }
                        }
                    }
                    sonarrRootPath = path;
                }
                else
                {
                    Console.WriteLine("Failed to connect to Sonarr for RootFolder Path");
                }

               
            }

        }
       
        //This method is used to get the root path of the Radarr server
        public static void SetRadarrRootPath()
        {
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("X-Api-Key", Xilften.radarrAPI);

                string rootFloderPath = "";
                string requestUrl = radarrIP + $"api/v3/rootfolder";

                var response = client.GetAsync(requestUrl).Result;
                var json = client.GetAsync(requestUrl).Result.Content.ReadAsStringAsync().Result;

                JArray objArray = JsonConvert.DeserializeObject<JArray>(json);
                string path = "";
                if (response.IsSuccessStatusCode)
                {
                    foreach (var obj in objArray)
                    {
                        if (obj != null)
                        {
                            try { path = obj["path"].ToString(); } catch { }
                            if (path != "")
                            {
                                break;
                            }
                        }
                    }
                    radarrRootPath = path;
                   // Console.WriteLine($"Radarr RootFolder Path Response Recieved as {path} ");
                }
                else
                {

                    Console.WriteLine("Failed to connect to Radarr for RootFolder Path");
                }


            }

        }

        //This Method is used to Write to the log file.
        public static async Task WriteToLog(string request)
        {

            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine($"[{DateTime.Now}]: {request}");
            }

        }

        //This method is used to read the log file and return the contents of the log file
        public static string ReadRequestLog()
        {
            if (File.Exists(logFilePath))
            {
                string logContents = File.ReadAllText(logFilePath);

                return logContents;
            }
            else
            {
                return null;
            }
        }

        //This method is used to save the request dictionary to a json file
        public static void SaveDictionaryToJson(Dictionary<DateTime, Request> dictionary)
        {
            // Serialize the dictionary to JSON
            string jsonString = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
           

            // Write the JSON string to a file
            File.WriteAllText(requestsFilePath, jsonString);

            WriteToLog("Request Database saved to Json.");
        }

        //This method is used to load the request dictionary from a json file
        public static Dictionary<DateTime, Request> LoadDictionaryFromJson(string filePath)
        {
            // Check if the file exists
            if (File.Exists(filePath))
            {
                // Read the JSON string from the file
                string jsonString = File.ReadAllText(filePath);

                // Deserialize the JSON string to a dictionary
                WriteToLog("Request Database loaded from Json.");
                return JsonConvert.DeserializeObject<Dictionary<DateTime, Request>>(jsonString);

            }
            else
            {
                // If the file doesn't exist, return an empty dictionary
                WriteToLog("Error Loading Request Database FIle does not exist.");
                return new Dictionary<DateTime, Request>();
            }
        }
        
        //This method is used to save the request dictionary to a json file on Close
        public static  void ProcessExitHandler(object sender, EventArgs e)
        {
            // Save dictionary to JSON file when the application exits
            WriteToLog("XilftenBot was Closed.");

            // Console.WriteLine(requests.ToString());
            SaveDictionaryToJson(requests);
            
        }

        //This method is used to save the request dictionary to a json file on Crash
        public static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            // Save dictionary to JSON file in case of unhandled exceptions
            WriteToLog($"XilftenBot Crashed Exception Object. {e.ExceptionObject.ToString}.");
            //Console.WriteLine(requests.ToString());
            SaveDictionaryToJson(requests);
        }
    }

    //This class is used to create a Request object that will be used to store the request made by the user
    public class Request
    {
        [JsonProperty("memberName")]
        public string memberName { get; set; }

        [JsonProperty("request")]
        public string request { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("dateMade")]
        public DateTime dateMade { get; set; }

        [JsonProperty("wasAdded")]
        public bool wasAdded { get; set; }
    }

}