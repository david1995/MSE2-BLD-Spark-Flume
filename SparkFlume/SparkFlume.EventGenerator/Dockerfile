### SYFOX - API ####
FROM ubuntu:17.10

# .NET Core Setup
RUN echo "#####   .NET Core SDK Setup   #####" && \
    wget -q https://packages.microsoft.com/config/ubuntu/17.10/packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get install apt-transport-https && \
    apt-get update && \
    apt-get install dotnet-sdk-2.1

CMD ["bash"]

RUN mkdir -p /usr/src/SparkFlume
WORKDIR /usr/src/SparkFlume

ADD entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

ADD . /usr/src/SparkFlume

# build & publish .NET Core app
RUN dotnet publish SparkFlume.EventGenerator/SparkFlume.EventGenerator.csproj -c Release

ENTRYPOINT ["/entrypoint.sh"]
