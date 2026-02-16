FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MindCalm.Services.Identity.API/MindCalm.Services.Identity.API.csproj", "MindCalm.Services.Identity.API/"]
RUN dotnet restore "MindCalm.Services.Identity.API/MindCalm.Services.Identity.API.csproj"
COPY . .
WORKDIR "/src/MindCalm.Services.Identity.API"
RUN dotnet build "./MindCalm.Services.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MindCalm.Services.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MindCalm.Services.Identity.API.dll"]
