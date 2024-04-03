var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder
  .AddProject<Projects.TvNoms_ApiService>("apiservice")
  .WithEnvironment("DOTNET_ROOT_X64", "/home/fergalm/.dotnet")
  .WithEnvironment("ASPNETCORE_URLS", "https://tvnoms.dev.fergl.ie:5001");

builder.Build().Run();
