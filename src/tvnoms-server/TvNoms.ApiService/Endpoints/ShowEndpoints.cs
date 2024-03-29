using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TvNoms.Core.DTO;
using TvNoms.Core.MediaLookup;
using TvNoms.Core.Models;
using TvNoms.Core.Utilities;

namespace TvNoms.Server.ApiService.Endpoints;

public class ShowEndpoints : Shared.Endpoints {
  public ShowEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
    : base(endpointRouteBuilder) {
  }

  public override void Configure() {
    var endpoints = MapGroup("/shows");
    endpoints.MapGet("/{type}/trending", GetTrending).RequireAuthorization();
  }

  public async Task<ShowPageModel> GetTrending(
    [FromServices] IMapper mapper,
    [FromServices] IModelBuilder modelBuilder,
    [FromServices] IShowLookupService showLookupService) {
    var trendingResults = await showLookupService.GetTrendingShows();
    var dto = mapper.Map<List<ShowDto>>(trendingResults.Items);

    var response = new Pageable<ShowDto>(
      trendingResults.Page,
      trendingResults.TotalPages,
      trendingResults.TotalResults,
      dto);
    return await modelBuilder.BuildAsync(response);
  }
}
