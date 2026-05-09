# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SmartAuth.WebAPI/SmartAuth.WebAPI.csproj", "SmartAuth.WebAPI/"]
COPY ["SmartAuth.Infrastructure/SmartAuth.Infrastructure.csproj", "SmartAuth.Infrastructure/"]
COPY ["SmartAuth.Application/SmartAuth.Application.csproj", "SmartAuth.Application/"]
COPY ["SmartAuth.Domain/SmartAuth.Domain.csproj", "SmartAuth.Domain/"]
COPY ["SmartAuth.SharedKernel/SmartAuth.SharedKernel.csproj", "SmartAuth.SharedKernel/"]
RUN dotnet restore "./SmartAuth.WebAPI/SmartAuth.WebAPI.csproj"
COPY . .
WORKDIR "/src/SmartAuth.WebAPI"
RUN dotnet build "./SmartAuth.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SmartAuth.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartAuth.WebAPI.dll"]