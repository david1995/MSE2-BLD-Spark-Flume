FROM python:3.6-stretch

RUN set -x \
    && apt-get update \
    && apt-get install wget openjdk-8-jdk-headless

# install spark
RUN mkdir /spark \
    && wget http://mirror.klaus-uwe.me/apache/spark/spark-2.3.0/spark-2.3.0-bin-hadoop2.7.tgz \
    && tar xzvf spark-2.3.0-bin-hadoop2.7.tgz -C /spark --strip-components=1

# setup python
RUN apt-get install -y python-pip \
    && pip install mysql-connector

EXPOSE 10001

RUN export PATH=$PATH:/spark/bin

WORKDIR /app

RUN wget http://central.maven.org/maven2/org/apache/spark/spark-streaming-flume-assembly_2.11/2.3.0/spark-streaming-flume-assembly_2.11-2.3.0.jar

COPY SparkFlume.Spark.py SparkFlume.Spark.py
COPY entrypoint.sh /entrypoint.sh

RUN chmod +x /entrypoint.sh
ENTRYPOINT ["/entrypoint.sh"]
