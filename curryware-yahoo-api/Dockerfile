FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["curryware-yahoo-api/curryware-yahoo-api.csproj", "curryware-yahoo-api/"]
RUN dotnet restore "curryware-yahoo-api/curryware-yahoo-api.csproj"
COPY . .
WORKDIR "/src/curryware-yahoo-api"
RUN dotnet build "curryware-yahoo-api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "curryware-yahoo-api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "curryware-yahoo-api.dll"]
