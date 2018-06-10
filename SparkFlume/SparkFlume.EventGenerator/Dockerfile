### SYFOX - API ####
FROM ubuntu:16.04

# .NET Core Setup
RUN echo "#####   .NET Core SDK Setup   #####" && \
    wget -q https://packages.microsoft.com/config/ubuntu/17.10/packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get install apt-transport-https && \
    apt-get update && \
    apt-get install dotnet-sdk-2.1


RUN mkdir -p /usr/src/SparkFlume
WORKDIR /usr/src/app

COPY pom.xml /usr/src/app/pom.xml
RUN mvn validate && \
mvn dependency:resolve && \
mvn dependency:resolve-plugins

ADD entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

ADD . /usr/src/SparkFlume
RUN dotnet build

EXPOSE 8000 8001

CMD ["/entrypoint.sh"]
