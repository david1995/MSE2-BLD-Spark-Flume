#!/bin/bash
echo "Starting spark"

exec /spark/bin/spark-submit --jars /app/spark-streaming-flume-assembly_2.11-2.3.0.jar /SparkFlume.Spark.py 127.0.0.1 10001
