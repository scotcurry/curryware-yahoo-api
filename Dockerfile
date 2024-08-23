FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ./curryware-yahoo-api ./
RUN dotnet build curryware-yahoo-api.csproj -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish curryware-yahoo-api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Final output
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "curryware-yahoo-api.dll"]
