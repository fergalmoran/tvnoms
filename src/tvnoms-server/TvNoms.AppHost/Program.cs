var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder
  .AddProject<Projects.TvNoms_ApiService>("apiservice")
  .WithEnvironment("ASPNETCORE_URLS", "https://tvnoms.dev.fergl.ie:5001");

builder
  .AddProject<Projects.TvNoms_Web>("webfrontend")
  .WithReference(apiService);

builder.Build().Run();
