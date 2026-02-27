using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvNoms.Core.MediaLookup;
using TvNoms.Server.Services.MediaLookup;

namespace TvNoms.Infrastructure.MediaLookup;

public static class ServiceCollectionExtensions {
  public static IServiceCollection AddMediaLookupServices(this IServiceCollection services, IConfiguration config) {
    services.AddTransient<IShowLookupService, TheMovieDbService>();
    services.AddHttpClient("themoviedb", c => {
      c.BaseAddress = new Uri("https://api.themoviedb.org/3/");
      c.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", config.GetSection("ApiKeys:TheMovieDb:Key").Value);
    });
    return services;
  }
}
