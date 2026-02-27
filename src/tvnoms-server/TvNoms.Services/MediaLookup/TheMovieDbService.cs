using Microsoft.Extensions.Configuration;
using TvNoms.Core.MediaLookup;
using TvNoms.Core.Extensions;

namespace TvNoms.Server.Services.MediaLookup;

public class TheMovieDbService(IConfiguration config, IHttpClientFactory httpClientFactory)
  : IShowLookupService {
  private readonly HttpClient _httpClient = httpClientFactory.CreateClient("themoviedb");

  public async Task<TheMovieDbResult<RemoteShowModel>> GetTrendingShows(CancellationToken token = default) {
    var response = await _httpClient.GetAsync($"trending/tv/week", token);
    var results = await response
      .Content
      .DeserializeHttpContent<TheMovieDbResult<RemoteShowModel>>(token);
    return results;
  }
}
