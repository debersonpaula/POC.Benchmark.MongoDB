    FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
    WORKDIR /app
    EXPOSE 80

    FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
    WORKDIR /src
    COPY . .
    ADD tmp/nuget-restore.tar.gz /root/.nuget
    ENV ASPNETCORE_ENVIRONMENT=Docker
    RUN dotnet restore "DataApi/DataApi.csproj" --source /root/.nuget/packages
    WORKDIR "/src/DataApi"
    RUN dotnet build "DataApi.csproj" -c Release -o /app/build --no-restore

    FROM build AS publish
    RUN dotnet publish "DataApi.csproj" -c Release -o /app/publish

    FROM base AS final
    WORKDIR /app
    COPY --from=publish /app/publish .
    ENTRYPOINT ["dotnet", "DataApi.dll"]

