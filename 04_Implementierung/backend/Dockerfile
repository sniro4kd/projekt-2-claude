# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY CatchTheRabbit.Core/*.csproj CatchTheRabbit.Core/
COPY CatchTheRabbit.Infrastructure/*.csproj CatchTheRabbit.Infrastructure/
COPY CatchTheRabbit.Api/*.csproj CatchTheRabbit.Api/
COPY CatchTheRabbit.sln .

RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish CatchTheRabbit.Api/CatchTheRabbit.Api.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create data directory for SQLite
RUN mkdir -p /app/data

COPY --from=build /app/publish .

# Environment variables
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/catchtherabbit.db"

EXPOSE 5000

ENTRYPOINT ["dotnet", "CatchTheRabbit.Api.dll"]
