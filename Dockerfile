# Dockerfile
ARG DOTNET_VERSION=10.0
ARG BUILD_CONFIGURATION=Production

# --- Stage 1: Build & Publish ---
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build

# Copy codebase
WORKDIR /src
COPY ["UrlShorten/", "UrlShorten/"]

# Restore dependencies and build
WORKDIR /src/UrlShorten
RUN dotnet restore "./UrlShorten.csproj"
RUN dotnet build "./UrlShorten.csproj" -c "$BUILD_CONFIGURATION" -o /app/build --no-restore

RUN dotnet publish "./UrlShorten.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish --no-build --no-restore

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} as runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet", "UrlShorten.dll" ]