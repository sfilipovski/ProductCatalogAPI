FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy the project file and restore any dependencies and publish the image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProductCatalogAPI.csproj", "."]
# Copy the rest of the files
RUN dotnet restore "ProductCatalogAPI.csproj"
COPY . .
WORKDIR /src
RUN dotnet publish "ProductCatalogAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProductCatalogAPI.dll"]