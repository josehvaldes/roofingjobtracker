# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["JobTracker.API/JobTracker.API.csproj", "JobTracker.API/"]
COPY ["JobTracker.Application/JobTracker.Application.csproj", "JobTracker.Application/"]
COPY ["JobTracker.Contracts/JobTracker.Contracts.csproj", "JobTracker.Contracts/"]
COPY ["JobTracker.Domain/JobTracker.Domain.csproj", "JobTracker.Domain/"]
COPY ["JobTracker.Infrastructure/JobTracker.Infrastructure.csproj", "JobTracker.Infrastructure/"]
COPY ["JobTracker.IntegrationEvents/JobTracker.IntegrationEvents.csproj", "JobTracker.IntegrationEvents/"]
COPY ["JobTracker.Seeder/JobTracker.Seeder.csproj", "JobTracker.Seeder/"]
RUN dotnet restore "./JobTracker.API/JobTracker.API.csproj"
COPY . .
WORKDIR "/src/JobTracker.API"
RUN dotnet build "./JobTracker.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JobTracker.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobTracker.API.dll"]