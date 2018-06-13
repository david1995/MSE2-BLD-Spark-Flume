FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY SparkFlume.EventGenerator.csproj SparkFlume.EventGenerator/
COPY ../SparkFlume.Common/SparkFlume.Common.csproj SparkFlume.Common/
RUN dotnet restore SparkFlume.EventGenerator/SparkFlume.EventGenerator.csproj
COPY . .
WORKDIR /src/SparkFlume.EventGenerator
RUN dotnet build SparkFlume.EventGenerator.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SparkFlume.EventGenerator.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["/entrypoint.sh"]
