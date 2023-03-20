#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WeatherForecast.App/WeatherForecast.App.csproj", "WeatherForecast.App/"]
COPY ["WeatherForecast.Domain/WeatherForecast.Domain.csproj", "WeatherForecast.Domain/"]
COPY ["WeatherForecast.Infrastructure/WeatherForecast.Infrastructure.csproj", "WeatherForecast.Infrastructure/"]
COPY ["WeatherForecast.Services/WeatherForecast.Services.csproj", "WeatherForecast.Services/"]
RUN dotnet restore "WeatherForecast.App/WeatherForecast.App.csproj"
COPY . .
WORKDIR "/src/WeatherForecast.App"
RUN dotnet build "WeatherForecast.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherForecast.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherForecast.App.dll"]