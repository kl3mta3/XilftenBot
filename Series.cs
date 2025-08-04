using Newtonsoft.Json;

namespace XilftenBot
{

   /// <summary>
    /// 
    /// This is the class that will be used to create a series objects
    /// by Deserializing the json response from the Sonarr api
    /// and sending that request to the Sonarr api to be added.
    /// 
    /// Advised to not change any thing inside this file.
    /// </summary>
   public partial class Series
    {
        [JsonProperty("addOptions")]
        public AddOptions addOptions { get; set; }

        [JsonProperty("added")]
        public string added { get; set; }

        [JsonProperty("alternateTitles")]
        public string[] alternateTitles { get; set; }

        [JsonProperty("CleanTitle")]
        public string CleanTitle { get; set; }

        [JsonProperty("ended")]
        public bool ended { get; set; }

        [JsonProperty("firstAired")]
        public string firstAired { get; set; }

        [JsonProperty("folder")]
        public string folder { get; set; }

        [JsonProperty("genres")]
        public string[] genres { get; set; }

        [JsonProperty("images")]
        public Image[] images { get; set; }

        [JsonProperty("languageProfileId")]
        public int languageProfileId { get; set; }

        [JsonProperty("monitored")]
        public bool monitored { get; set; }

        [JsonProperty("network")]
        public string network {  get; set; }

        [JsonProperty("originalLanguage")]
        public OriginalLanguage originalLanguage { get; set; }

        [JsonProperty("overview")]
        public string overview { get; set; }

        [JsonProperty("qualityProfileId")]
        public int qualityProfileId { get; set; }

        [JsonProperty("ratings")]
        public Ratings ratings { get; set; }

        [JsonProperty("remotePoster")]
        public string remotePoster { get; set; }

        [JsonProperty("rootFolderPath")]
        public string rootFolderPath { get; set; }

        [JsonProperty("runtime")]
        public int runtime { get; set; }

        [JsonProperty("seasonFolder")]
        public bool seasonFolder { get; set; }

        [JsonProperty("seasons")]
        public Season[] seasons { get; set; }

        [JsonProperty("seriesType")]
        public string seriesType { get; set; }

        [JsonProperty("sortTitle")]
        public string sortTitle { get; set; }

        [JsonProperty("statistic")]
        public Statistics statistics { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("tags")]
        public int[] tags { get; set; }
        
        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("titleSlug")]
        public string titleSlug { get; set; }

        [JsonProperty("tvMazeId")]
        public int tvMazeId { get; set; }

        [JsonProperty("tvRageId")]
        public int tvRageId { get; set; }

        [JsonProperty("tvdbId")]
        public int tvdbId { get; set; }

        [JsonProperty("useSceneNumbering")]
        public bool useSceneNumbering { get; set; }

        [JsonProperty("year")]
        public int year { get; set; }

        //Sets the add options for the series (needed for adding a show)
        public void SetAddOptions(bool searchForMissing, string monitor, bool searchForCutoffUnmetEpisodes)
        {
            bool _searchForMissing ;
            string _monitor = "";
            bool _searchForCutoffUnmetEpisodes ;
            if (searchForMissing == null)
            {
                _searchForMissing = false;
            }
            else
            {
                _searchForMissing = searchForMissing;
            }

            if (monitor == "")
            {
                _monitor = "all";
            }
            else
            {
                _monitor = monitor;
            }

            if (searchForCutoffUnmetEpisodes == null)
            {
                _searchForCutoffUnmetEpisodes = false;
            }
            else
            {
                _searchForCutoffUnmetEpisodes = searchForCutoffUnmetEpisodes;
            }

            AddOptions _addOptions = new AddOptions();
            _addOptions.SetAddOptions(_searchForMissing, _monitor, _searchForCutoffUnmetEpisodes);

            addOptions=_addOptions;
        }
    }

   /// <summary>
    /// 
    /// All of the following are needed to Deserialize the json response from the radarr api.
    /// I would not recommend changing any of the following classes.
    /// 
    /// </summary>

   public partial class AddOptions
    {
        [JsonProperty("monitor")]
        public  string monitor  { get; set; }

        [JsonProperty("searchForMissingEpisodes")]
        public  bool searchForMissingEpisodes  { get; set; }

        [JsonProperty("searchForCutoffUnmetEpisodes")]
        public  bool searchForCutoffUnmetEpisodes  { get; set; }


        public void SetAddOptions(bool searchForMissing, string _monitor, bool searchForCutoff)
        {
            searchForMissingEpisodes = searchForMissing;
            monitor = _monitor;
            searchForCutoffUnmetEpisodes = searchForCutoff;

        }



    }

   public partial class Image
    {
        [JsonProperty("coverType")]
        public  string coverType { get; set; }

        [JsonProperty("remoteUrl")]
        public  string remoteUrl { get; set; }

        [JsonProperty("url")]
        public  string url { get; set; }
    }

   public partial class OriginalLanguage
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

    }

   public partial class Ratings
    {
        [JsonProperty("value")]
        public  float value { get; set; }

        [JsonProperty("votes")]
        public  int votes { get; set; }

    }

   public partial class Season
    {
        [JsonProperty("monitored")]
        public  bool monitored { get; set; }
        [JsonProperty("seasonNumber")]
        public  int seasonNumber { get; set; }
    }

   public partial class Statistics
    {
        [JsonProperty("episodeCount")]
        public  int episodeCount { get; set; }

        [JsonProperty("episodeFileCount")]
        public  int episodeFileCount { get; set; }

        [JsonProperty("percentOfEpisodes")]
        public  int percentOfEpisodes { get; set; }

        [JsonProperty("seasonCount")]
        public  int seasonCount { get; set; }

        [JsonProperty("sizeOnDisk")]
        public  int sizeOnDisk { get; set; }

        [JsonProperty("totalEpisodeCount")]
        public  int totalEpisodeCount { get; set; }
    }



}


