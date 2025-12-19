
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MyTestApp/MyTestApp/MyTestApp.csproj", "MyTestApp/MyTestApp/"]
COPY ["MyTestApp/MyTestApp.Client/MyTestApp.Client.csproj", "MyTestApp/MyTestApp.Client/"]
RUN dotnet restore "./MyTestApp/MyTestApp/MyTestApp.csproj"
COPY . .
WORKDIR "/src/MyTestApp/MyTestApp"
RUN dotnet build "./MyTestApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyTestApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyTestApp.dll"]