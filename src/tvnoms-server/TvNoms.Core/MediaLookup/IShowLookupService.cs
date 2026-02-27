using System.Text.Json.Serialization;
using AbstractProfile = AutoMapper.Profile;

namespace TvNoms.Core.MediaLookup;

public class TheMovieDbResult<T> : AbstractProfile {
  [JsonPropertyName("page")] public int Page { get; set; }
  [JsonPropertyName("total_pages")] public int TotalPages { get; set; }
  [JsonPropertyName("total_results")] public int TotalResults { get; set; }
  [JsonPropertyName("results")] public List<T> Items { get; set; }
}

public class Season {
  [JsonPropertyName("id")] public int Id { get; set; }
  [JsonPropertyName("air_date")] public string AirDate { get; set; }
  [JsonPropertyName("episode_count")] public int EpisodeCount { get; set; }
  [JsonPropertyName("name")] public string Name { get; set; }
  [JsonPropertyName("overview")] public string Overview { get; set; }
  [JsonPropertyName("poster_path")] public string PosterPath { get; set; }
  [JsonPropertyName("season_number")] public int SeasonNumber { get; set; }
  [JsonPropertyName("vote_average")] public double VoteAverage { get; set; }
}

public class RemoteShowModel {
  [JsonPropertyName("id")] public long TheMovieDbId { get; set; }
  [JsonPropertyName("name")] public string Title { get; set; }
  [JsonPropertyName("overview")] public string Description { get; set; }

  [JsonPropertyName("popularity")] public float Popularity { get; set; }
  [JsonPropertyName("vote_average")] public float AverageVote { get; set; }
  [JsonPropertyName("vote_count")] public long VoteCount { get; set; }
  [JsonPropertyName("backdrop_path")] public string BackdropImage { get; set; }
  [JsonPropertyName("poster_path")] public string PosterImage { get; set; }
  [JsonPropertyName("first_air_date")] public DateTime FirstAired { get; set; }
  [JsonPropertyName("adult")] public bool IsAdult { get; set; }

  [JsonPropertyName("number_of_seasons")]
  public int NumberOfSeasons { get; set; }

  [JsonPropertyName("number_of_episodes")]
  public int NumberOfEpisodes { get; set; }

  public List<Season> Seasons { get; set; }
  [JsonPropertyName("genre_ids")] public long[] GenreIds { get; set; }
}

public interface IShowLookupService {
  public Task<TheMovieDbResult<RemoteShowModel>> GetTrendingShows(CancellationToken token = default);
}
