FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY SparkFlume.Output.csproj SparkFlume.Output.csproj

RUN dotnet restore SparkFlume.Output/SparkFlume.Output.csproj
COPY . .

RUN dotnet build SparkFlume.Output.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SparkFlume.Output.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
RUN chmod +x /entrypoint.sh
ENTRYPOINT ["/entrypoint.sh"]
