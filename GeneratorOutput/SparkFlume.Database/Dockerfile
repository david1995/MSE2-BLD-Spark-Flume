# Copyright (c) 2017, Oracle and/or its affiliates. All rights reserved.
#
# This program is free software; you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation; version 2 of the License.
#
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with this program; if not, write to the Free Software
# Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301 USA
FROM oraclelinux:7-slim

ARG PACKAGE_URL=https://repo.mysql.com/yum/mysql-5.7-community/docker/x86_64/mysql-community-server-minimal-5.7.22-1.el7.x86_64.rpm
ARG PACKAGE_URL_SHELL=https://repo.mysql.com/yum/mysql-tools-community/el/7/x86_64/mysql-shell-1.0.11-1.el7.x86_64.rpm

# Install server
RUN rpmkeys --import https://repo.mysql.com/RPM-GPG-KEY-mysql \
  && yum install -y $PACKAGE_URL $PACKAGE_URL_SHELL libpwquality \
  && yum clean all \
  && mkdir /docker-entrypoint-initdb.d

VOLUME /var/lib/mysql

ENV MYSQL_USER docker
ENV MYSQL_PASSWORD docker
ENV MYSQL_ROOT_PASSWORD docker
ENV MYSQL_DATABASE statistics

COPY docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh
RUN sed -Ei 's/^(bind-address|log)/#&/' /etc/my.cnf
COPY CreateDatabase.sql /docker-entrypoint-initdb.d/CreateDatabase.sql
ENTRYPOINT ["/docker-entrypoint.sh"]
HEALTHCHECK CMD /healthcheck.sh

EXPOSE 3306 3307
CMD ["mysqld"]