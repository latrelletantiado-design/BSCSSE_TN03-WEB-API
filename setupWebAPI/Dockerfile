# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY setupWebAPI.csproj .
RUN dotnet restore setupWebAPI.csproj

# Copy the rest of the code
COPY . .
RUN dotnet publish setupWebAPI.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080
ENTRYPOINT ["dotnet", "setupWebAPI.dll"]
