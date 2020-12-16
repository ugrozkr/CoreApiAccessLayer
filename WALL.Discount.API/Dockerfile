#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WALL.Discount.API/WALL.Discount.API.csproj", "WALL.Discount.API/"]
RUN dotnet restore "WALL.Discount.API/WALL.Discount.API.csproj"
COPY . .
WORKDIR "/src/WALL.Discount.API"
RUN dotnet build "WALL.Discount.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WALL.Discount.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WALL.Discount.API.dll"]