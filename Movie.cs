using Newtonsoft.Json;


/// <summary>
/// 
/// This is the class that will be used to create a movie object
/// by Deserializing the json response from the radarr api
/// and sending that request to the radarr api to be added.
/// 
/// Advised to not change any thing inside this file.
/// </summary>
public partial class Movie
{
    [JsonProperty("addOptions")]
    public AddOptions addOptions { get; set; }

    [JsonProperty("added")]
    public string added { get; set; }

    [JsonProperty("alternateTitles")]
    public AlternateTitle[] alternateTitles { get; set; }

    [JsonProperty("certification")]
    public string certification { get; set; }

    [JsonProperty("cleanTitle")]
    public string cleanTitle { get; set; }

    [JsonProperty("collection")]
    public Collection collection { get; set; }

    [JsonProperty("folder")]
    public string folder { get; set; }

    [JsonProperty("folderName")]
    public string folderName { get; set; }

    [JsonProperty("genres")]
    public string[] genres { get; set; }

    [JsonProperty("hasFile")]
    public bool hasFile { get; set; }

    [JsonProperty("id")]
    public int id { get; set; }

    [JsonProperty("images")]
    public Image[] images { get; set; }

    [JsonProperty("imdbId")]
    public string imdbId { get; set; }

    [JsonProperty("inCinemas")]
    public string inCinemas { get; set; }

    [JsonProperty("isAvailable")]
    public bool isAvailable { get; set; }

    [JsonProperty("minimumAvailability")]
    public string minimumAvailability { get; set; }

    [JsonProperty("monitored")]
    public bool monitored { get; set; }

    [JsonProperty("originalLanguage")]
    public OriginalLanguage originalLanguage { get; set; }

    [JsonProperty("originalTitle")]
    public string originalTitle { get; set; }

    [JsonProperty("overview")]
    public string overview { get; set; }

    [JsonProperty("popularity")]
    public decimal popularity { get; set; }

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

    [JsonProperty("secondaryYearSourceId")]
    public int secondaryYearSourceId { get; set; }


    [JsonProperty("sortTitle")]
    public string sortTitle { get; set; }

    [JsonProperty("studio")]
    public string studio { get; set; }

    [JsonProperty("tags")]
    public int[] tags { get; set; }

    [JsonProperty("title")]
    public string title { get; set; }

    [JsonProperty("titleSlug")]
    public string titleSlug { get; set; }

    [JsonProperty("tmdbId")]
    public int tmdbId { get; set; }

    [JsonProperty("website")]
    public string website { get; set; }

    [JsonProperty("year")]
    public int year { get; set; }

    [JsonProperty("youTubeTrailerId")]
    public string youTubeTrailerId { get; set; }



    //Sets the add options for the movie (needed for adding a movie)
    public void SetAddOptions(bool searchForMovie, string monitor)
    {
        bool _searchForMovie;
        string _monitor = "";
        bool _searchForCutoffUnmetEpisodes;
        if (searchForMovie == null)
        {
            _searchForMovie = true;
        }
        else
        {
            _searchForMovie = searchForMovie;
        }

        if (monitor == "")
        {
            _monitor = "movieOnly";
        }
        else
        {
            _monitor = monitor;
        }

        monitored = true;

        AddOptions _addOptions = new AddOptions();
        _addOptions.SetAddOptions(_searchForMovie, _monitor);

        addOptions = _addOptions;
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
    public string monitor { get; set; }

    [JsonProperty("searchForMovie")]
    public bool searchForMovie{ get; set; }


    public void SetAddOptions(bool _searchForMovie, string _monitor)
    {
        searchForMovie = _searchForMovie;
        monitor = _monitor;
       
    }

}

public partial class Image
{
    [JsonProperty("coverType")]
    public string coverType { get; set; }

    [JsonProperty("remoteUrl")]
    public string remoteUrl { get; set; }

    [JsonProperty("url")]
    public string url { get; set; }
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
    public int value { get; set; }

    [JsonProperty("votes")]
    public int votes { get; set; }

}

public partial class Season
{
    [JsonProperty("monitored")]
    public bool monitored { get; set; }
    [JsonProperty("seasonNumber")]
    public int seasonNumber { get; set; }
}

public partial class Collection
{
    [JsonProperty("added")]
    public string added { get; set; }

    [JsonProperty("id")]
    public int id { get; set; }

    [JsonProperty("images")]
    public Image[] images  { get; set; }

    [JsonProperty("minimumAvailability")]
    public string minimumAvailability { get; set; }

    [JsonProperty("monitored")]
    public bool monitored { get; set; }

    [JsonProperty("qualityProfileId")]
    public int qualityProfileId { get; set; }

    [JsonProperty("tags")]
    public int[] tags { get; set; }

    [JsonProperty("title")]
    public string title { get; set; }

    [JsonProperty("tmdbId")]
    public int tmdbId { get; set; }

}

public partial class AlternateTitle
{
    [JsonProperty("sourceType")]
    public string sourceType { get; set; }

    [JsonProperty("movieMetadataId")]
    public int movieMetadataId { get; set; }

    [JsonProperty("title")]
    public string title { get; set; }


}

