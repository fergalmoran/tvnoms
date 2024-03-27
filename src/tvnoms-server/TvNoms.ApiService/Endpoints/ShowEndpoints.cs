using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TvNoms.Core.MediaLookup;

namespace TvNoms.Server.ApiService.Endpoints;

[EnableCors("WebAppCors")]
public class ShowEndpoints : Shared.Endpoints {
  public ShowEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
    : base(endpointRouteBuilder) {
  }

  public override void Configure() {
    var endpoints = MapGroup("/shows");

    endpoints.MapGet("/{type}/trending", GetTrending).RequireAuthorization();
  }

  public async Task<IResult> GetTrending([FromServices] IShowLookupService showLookupService) {
    return Results.Ok(await showLookupService.GetTrendingShows());
  }
}
