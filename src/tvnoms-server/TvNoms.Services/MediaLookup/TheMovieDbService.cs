using Microsoft.Extensions.Configuration;
using TvNoms.Core.MediaLookup;
using TvNoms.Core.Extensions;

namespace TvNoms.Server.Services.MediaLookup;

public class TheMovieDbService(IConfiguration config, IHttpClientFactory httpClientFactory)
  : IShowLookupService {
  private readonly HttpClient _httpClient = httpClientFactory.CreateClient("themoviedb");
  private readonly string? _apiKey = config.GetSection("ApiKeys:TheMovieDb:Key").Value;

  public async Task<TheMovieDbResult<RemoteShowModel>> GetTrendingShows(CancellationToken token = default) {
    var response = await _httpClient.GetAsync($"trending/tv/week?api_key={_apiKey}", token);
    var results = await response
      .Content
      .DeserializeHttpContent<TheMovieDbResult<RemoteShowModel>>(token);
    return results;
  }
}
