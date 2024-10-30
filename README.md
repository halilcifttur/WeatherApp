# WeatherApp

A .NET-based web application for retrieving and displaying weather information for various cities, with support for favorite cities, caching, and batch processing of weather requests. This project fulfills the requirements of the case project, implementing an ASP.NET MVC application integrated with PostgreSQL and Redis.

---

## Project Overview

WeatherApp allows users to:
- Enter city names and retrieve their average weather data.
- Add cities to their favorite list for quick access.
- View the average weather for all favorite cities and identify the hottest and coldest among them.
- Cache weather data using Redis to reduce the number of external API calls.
- Utilize batching to minimize redundant calls to external weather APIs.

### Key Features

- **ASP.NET MVC UI**: A user-friendly interface that enables interaction with weather data.
- **Favorite Cities**: Allows users to save favorite cities and view their weather details.
- **Batching API Calls**: Group multiple requests for the same location within a 5-second window to optimize API usage.
- **PostgreSQL Database**: Store cities and favorites data persistently.
- **Redis Cache**: Cache weather data to reduce API calls for the same city.
- **Serilog Logging**: Capture application logs to facilitate error tracking and debugging.
- **Dockerized Environment**: Easily set up and run all services (web app, PostgreSQL, Redis) with Docker Compose.
- **Error Handling**: Graceful handling of API errors and database connection issues.
- **Unit Tests**: Thorough unit tests for key components.

---

## Prerequisites

- **Visual Studio 2022** or later (required for building and running the application).
- **Docker Desktop** (required for running PostgreSQL and Redis containers).
- **.NET SDK 8.0**

---

## Getting Started

### 1. Clone the Repository

```
git clone https://github.com/yourusername/WeatherApp.git
cd WeatherApp
```
### 2. Open the Project in Visual Studio
Launch Visual Studio.
Click on File -> Open -> Project/Solution.
Navigate to the WeatherApp folder, select the WeatherApp.sln file, and click Open.

### 3. Build and Run the Application

#### Docker Setup

This project uses Docker Compose to set up PostgreSQL and Redis. Follow these steps:

```
1. Make sure Docker Desktop is running.
2. In Visual Studio’s Solution Explorer, locate docker-compose.yml and docker-compose.override.yml in the project root.
3. Right-click on docker-compose and select Set as Startup Project.
4. Click Start (or press F5) to build and run the Docker containers.
```
Docker will set up three containers:

```
weatherapp.web: The ASP.NET Core MVC application.
weatherapp.database: PostgreSQL database.
weatherapp.cache: Redis cache.
```

#### Apply Migrations
The database is configured to automatically apply migrations on startup. If you encounter issues, ensure the database container is running and re-run the application.

### 4. Running the Application in Visual Studio

```
1. Set docker-compose as the Startup Project in Visual Studio.
2. Press F5 to start the application.
```

Once the application starts, it will be accessible at:

 - http://localhost:5000 (HTTP)
 - https://localhost:5001 (HTTPS)

## Usage

```
1. Enter City Names: Go to the main page and enter one or multiple city names (comma-separated) to view their weather data.
2. Add Favorite Cities: Add cities to your favorites list, accessible from the favorites page.
3. View Favorite Cities: See all favorite cities and view the hottest and coldest cities among them.
```

## Configuration

### API Keys

The application uses two weather APIs: WeatherAPI and WeatherStack. These API keys are pre-configured in appsettings.json:

```
"WeatherApiSettings": {
  "WeatherApiUrl": "http://api.weatherapi.com/v1/forecast.json",
  "WeatherApiKey": "your_weatherapi_key",
  "WeatherStackApiUrl": "http://api.weatherstack.com/current",
  "WeatherStackApiKey": "your_weatherstack_key"
}
```

Replace `"your_weatherapi_key"` and `"your_weatherstack_key"` with valid API keys.

## Project Structure

```
WeatherApp
├── src
│   ├── WeatherApp.Web             # ASP.NET MVC Web Application
│   ├── WeatherApp.Application     # Application layer for business logic
│   ├── WeatherApp.Domain          # Domain layer for core entities and interfaces
│   └── WeatherApp.Infrastructure   # Infrastructure layer for data access and services
├── test
│   └── WeatherApp.Test            # Unit tests for application components
└── docker-compose.yml             # Docker Compose configuration
```


## Running Unit Tests

```
1. In Visual Studio, open the Test Explorer.
2. Click on Run All to execute all unit tests.
```

## Known Issues
- **Data Incompatibility:** If you encounter errors related to database incompatibility, clear the containers/weather-db volume directory to reset the PostgreSQL data.
- **Redis Connection:** Ensure Redis is running in Docker, and the connection string matches the configuration in appsettings.json.

## Additional Notes
- This project follows Domain-Driven Design (DDD) and SOLID principles.
- Logging is implemented using Serilog for enhanced observability.
- Error Handling is used to ensure a smooth user experience and graceful recovery from errors.
- Caching is implemented to reduce external API calls, enhancing application performance.