#!/bin/bash

exec dotnet SparkFlume.EventGenerator.dll generate --demo --purchasestarget http://127.0.0.1:28000 --viewstarget http://127.0.0.1:28001
