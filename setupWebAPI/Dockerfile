# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy sln if you have one; otherwise copy the csproj directly
COPY *.sln . 2>/dev/null || true
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Kestrel in official images listens on 8080 by default (http://+:8080),
# which Render can autodetect and route to. No hardcoded port needed.
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "setupWebAPI.dll"]
