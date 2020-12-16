FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["WALL.Catalog.API/WALL.Catalog.API.csproj", "WALL.Catalog.API/"]
RUN dotnet restore "WALL.Catalog.API/WALL.Catalog.API.csproj"
COPY . .
WORKDIR "/src/WALL.Catalog.API"
RUN dotnet build "WALL.Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WALL.Catalog.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WALL.Catalog.API.dll"]