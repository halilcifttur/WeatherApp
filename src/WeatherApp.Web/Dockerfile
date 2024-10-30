# Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/WeatherApp.Web/WeatherApp.Web.csproj", "src/WeatherApp.Web/"]
COPY ["src/WeatherApp.Application/WeatherApp.Application.csproj", "src/WeatherApp.Application/"]
COPY ["src/WeatherApp.Domain/WeatherApp.Domain.csproj", "src/WeatherApp.Domain/"]
COPY ["src/WeatherApp.Infrastructure/WeatherApp.Infrastructure.csproj", "src/WeatherApp.Infrastructure/"]
RUN dotnet restore "src/WeatherApp.Web/WeatherApp.Web.csproj"
COPY . .
WORKDIR "/src/src/WeatherApp.Web"
RUN dotnet build "WeatherApp.Web.csproj" -c Release -o /app/build

# Stage 2: Publish Stage
FROM build AS publish
RUN dotnet publish "WeatherApp.Web.csproj" -c Release -o /app/publish

# Stage 3: Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherApp.Web.dll"]
