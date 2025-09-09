# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution and project files
COPY setupWebAPI.sln ./
COPY setupWebAPI/setupWebAPI.csproj ./setupWebAPI/

# Restore dependencies
RUN dotnet restore setupWebAPI.sln

# Copy the rest of the source code
COPY . .
WORKDIR /src/setupWebAPI
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "setupWebAPI.dll"]
