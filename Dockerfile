FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
LABEL maintainer="scotcurry4@gmail.com"

## Need to get all of the curryware-yahoo dependencies
WORKDIR /src
COPY *.sln ./
COPY curryware-data-models/curryware-data-models.csproj curryware-data-models/
COPY curryware-fantasy-command-line-tool/curryware-fantasy-command-line-tool.csproj curryware-fantasy-command-line-tool/
COPY curryware-fantasy-command-line-tool-tests/curryware-fantasy-command-line-tool-tests.csproj curryware-fantasy-command-line-tool-tests/
COPY curryware-kafka-producer-library/curryware-kafka-producer-library.csproj curryware-kafka-producer-library/
COPY curryware-kafka-producer-library-tests/curryware-kafka-producer-library-tests.csproj curryware-kafka-producer-library-tests/
COPY curryware-postgres-library/curryware-postgres-library.csproj curryware-postgres-library/
COPY curryware-postgres-library-tests/curryware-postgres-library-tests.csproj curryware-postgres-library-tests/
COPY curryware-yahoo-api/curryware-yahoo-api.csproj curryware-yahoo-api/
COPY curryware-yahoo-api-tests/curryware-yahoo-api-tests.csproj curryware-yahoo-api-tests/
COPY curryware-yahoo-parsing-library/curryware-yahoo-parsing-library.csproj curryware-yahoo-parsing-library/
COPY curryware-yahoo-parsing-library-tests/curryware-yahoo-parsing-library-tests.csproj curryware-yahoo-parsing-library-tests/

RUN dotnet --version
RUN dotnet restore
COPY . ./

RUN dotnet publish curryware-yahoo-api/curryware-yahoo-api.csproj -c Release -o /src/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
WORKDIR /app
COPY --from=build /src/publish /app

ARG DD_GIT_REPOSITORY_URL
ARG DD_GIT_COMMIT_SHA
ENV DD_GIT_REPOSITORY_URL=${DD_GIT_REPOSITORY_URL} 
ENV DD_GIT_COMMIT_SHA=${DD_GIT_COMMIT_SHA}

## Final output
WORKDIR /app
EXPOSE 8087
ENV ASPNETCORE_HTTP_PORTS=8087
ENV DD_DYNAMIC_INSTRUMENTATION_ENABLED=true
ENTRYPOINT ["dotnet", "curryware-yahoo-api.dll"]
