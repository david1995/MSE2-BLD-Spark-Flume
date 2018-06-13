FROM debian:stretch

RUN set -x && apt-get update && apt-get install -y wget openjdk-8-jdk-headless

RUN set -x \
    && mkdir /flume \
    && curl -fsSL http://archive.apache.org/dist/flume/1.8.0/apache-flume-1.8.0-bin.tar.gz \
    | tar xvfz - -C /flume --strip-components=1

EXPOSE 28000
EXPOSE 28001

COPY flume.conf /flume/conf/flume.conf
COPY flume-env.sh /flume/conf/flume-env.sh
COPY entrypoint.sh /entrypoint.sh

RUN chmod +x /entrypoint.sh
ENTRYPOINT ["/entrypoint.sh"]
