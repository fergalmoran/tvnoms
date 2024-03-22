#!/usr/bin/env bash

rm -rfv ./TvNoms.Data/Migrations

dotnet ef database drop --force --startup-project ./TvNoms.ApiService/TvNoms.ApiService.csproj --project ./TvNoms.Data/TvNoms.Data.csproj
dotnet ef migrations add "Initial" --startup-project ./TvNoms.ApiService/TvNoms.ApiService.csproj --project ./TvNoms.Data/TvNoms.Data.csproj
dotnet ef database update --startup-project ./TvNoms.ApiService/TvNoms.ApiService.csproj --project ./TvNoms.Data/TvNoms.Data.csproj
