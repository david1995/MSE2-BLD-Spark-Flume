#!/usr/bin/env bash

cd SparkFlume.EventGenerator/bin/Release/netcoreapp2.0/publish/
dotnet SparkFlume.EventGenerator.dll --PurchaseTarget 127.0.0.1:8000 --ViewTarget 127.0.0.1:8001
