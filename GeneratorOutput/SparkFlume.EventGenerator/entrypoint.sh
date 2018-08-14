#!/bin/bash

exec dotnet SparkFlume.EventGenerator.dll generate --purchasestarget http://flume:28001 --viewstarget http://flume:28000
