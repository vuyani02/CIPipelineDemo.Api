# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy solution and restore
COPY *.sln .
COPY src/ src/
COPY test/ test/
RUN dotnet restore

# Build and publish
RUN dotnet publish src/Acme.SimpleTaskApp.Web.Host/Acme.SimpleTaskApp.Web.Host.csproj \
    -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 44311
ENTRYPOINT ["dotnet", "Acme.SimpleTaskApp.Web.Host.dll"]