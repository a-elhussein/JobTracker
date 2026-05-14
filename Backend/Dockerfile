# ── Stage 1: Build ──────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Backend.sln .
COPY Backend.API/Backend.API.csproj         ./Backend.API/
COPY Backend.Core/Backend.Core.csproj       ./Backend.Core/
COPY Backend.Infrastructure/Backend.Infrastructure.csproj ./Backend.Infrastructure/

RUN dotnet restore

COPY . .
RUN dotnet publish Backend.API/Backend.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ── Stage 2: Runtime ────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Backend.API.dll"]