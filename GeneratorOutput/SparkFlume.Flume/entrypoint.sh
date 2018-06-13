#!/bin/bash
echo "Starting flume agent"
exec /flume/bin/flume-ng agent -c /flume/conf -f /flume/conf/flume.conf -n agent -Dflume.root.logger=INFO,console
