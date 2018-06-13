#!/bin/bash

exec dotnet SparkFlume.Output.dll --databaseserver 127.0.0.1:3306 --databasename Products
