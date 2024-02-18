using Newtonsoft.Json;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Series
{
    public class Series
    {
        public int id;
        public string title;
        public AlternateTitle alternateTitle;
        public Image image;
        public SeasonStatistic seasonStatistic;
        public Season season;
        public Rating rating;
        public Statistics statistics;
        public AddOptions addOptions;
        public Root root;

        public Series()
        {

        }

        public static Series FromJson(string json)
        {

            try
            {
                return JsonConvert.DeserializeObject<Series>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deserializing JSON: {ex.Message}");
                return null;
            }

        }
    }
    public class AlternateTitle
    {
        public string Title { get; set; }
        public int SeasonNumber { get; set; }
        public int SceneSeasonNumber { get; set; }
        public string SceneOrigin { get; set; }
        public string Comment { get; set; }


        public static AlternateTitle FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<AlternateTitle>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deserializing JSON: {ex.Message}");
                return null;
            }
        }
    }
    public class Image
    {
        public string CoverType { get; set; }
        public string Url { get; set; }
        public string RemoteUrl { get; set; }

        public static Image FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Image>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deserializing JSON: {ex.Message}");
                return null;
            }
        }
    }

    public class SeasonStatistic
    {
        public DateTime NextAiring { get; set; }
        public DateTime PreviousAiring { get; set; }
        public int EpisodeFileCount { get; set; }
        public int EpisodeCount { get; set; }
        public int TotalEpisodeCount { get; set; }
        public int SizeOnDisk { get; set; }
        public List<string> ReleaseGroups { get; set; }

        public static SeasonStatistic FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<SeasonStatistic>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deserializing JSON: {ex.Message}");
                return null;
            }
        }
    }

    public class Season
    {
        public int SeasonNumber { get; set; }
        public bool Monitored { get; set; }
        public SeasonStatistic Statistics { get; set; }
        public List<Image> Images { get; set; }

        public static Season FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Season>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deserializing JSON: {ex.Message}");
                return null;
            }
        }

    }

    public class Rating
    {
        public int Votes { get; set; }
        public int Value { get; set; }

        public static Rating FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Rating>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deserializing JSON: {ex.Message}");
                return null;
            }
        }


    }

    public class Statistics
    {
        public int SeasonCount { get; set; }
        public int EpisodeFileCount { get; set; }
        public int EpisodeCount { get; set; }
        public int TotalEpisodeCount { get; set; }
        public int SizeOnDisk { get; set; }
        public List<string> ReleaseGroups { get; set; }


        public static Statistics FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Statistics>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deserializing JSON: {ex.Message}");
                return null;
            }
        }




    }

    public class AddOptions
    {
        public bool IgnoreEpisodesWithFiles { get; set; }
        public bool IgnoreEpisodesWithoutFiles { get; set; }
        public string Monitor { get; set; }
        public bool SearchForMissingEpisodes { get; set; }
        public bool SearchForCutoffUnmetEpisodes { get; set; }

        public static AddOptions FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<AddOptions>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deserializing JSON: {ex.Message}");
                return null;
            }
        }



    }

    public class Root
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<AlternateTitle> AlternateTitles { get; set; }
        public string SortTitle { get; set; }
        public string Status { get; set; }
        public string ProfileName { get; set; }
        public string Overview { get; set; }
        public DateTime NextAiring { get; set; }
        public DateTime PreviousAiring { get; set; }
        public string Network { get; set; }
        public string AirTime { get; set; }
        public List<Image> Images { get; set; }
        public string OriginalLanguage { get; set; }
        public string RemotePoster { get; set; }
        public List<Season> Seasons { get; set; }
        public int Year { get; set; }
        public string Path { get; set; }
        public int QualityProfileId { get; set; }
        public bool SeasonFolder { get; set; }
        public bool Monitored { get; set; }
        public string MonitorNewItems { get; set; }
        public bool UseSceneNumbering { get; set; }
        public int Runtime { get; set; }
        public int TvdbId { get; set; }
        public int TvRageId { get; set; }
        public int TvMazeId { get; set; }
        public DateTime FirstAired { get; set; }
        public DateTime LastAired { get; set; }
        public string SeriesType { get; set; }
        public string CleanTitle { get; set; }
        public string ImdbId { get; set; }
        public string TitleSlug { get; set; }
        public string RootFolderPath { get; set; }
        public string Folder { get; set; }
        public string Certification { get; set; }
        public List<string> Genres { get; set; }
        public List<int> Tags { get; set; }
        public DateTime Added { get; set; }
        public AddOptions AddOptions { get; set; }
        public Rating Ratings { get; set; }
        public Statistics Statistics { get; set; }
        public bool EpisodesChanged { get; set; }


        public static Root FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Root>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deserializing JSON: {ex.Message}");
                return null;
            }
        }
    }
}


