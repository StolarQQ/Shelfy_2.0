# Shelfy

[![Platform](https://img.shields.io/badge/Platform-.NET%20Core%202.2-green.svg)](https://dotnet.microsoft.com/download)

Branch | Build Status
------------ | -------------
Master | [![Build Status](https://travis-ci.org/StolarQQ/Shelfy.svg?branch=master)](https://travis-ci.org/StolarQQ/Shelfy)

## What is Shelfy ?

Shelfy is 

## Prerequisite
In order to run Shelfy, you need to have installed:
+ [.NET Core SDK 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2)
+ [MongoDB](https://www.mongodb.com/download-center/community)

## Configuration


## How to start application
```
git clone https://github.com/StolarQQ/Shelfy.git
cd src/Shelfy.Api
dotnet restore 
dotnet run -urls "http://*:5001"
```
By default application will be available under https://localhost:5001

## Testing HTTP requests
You can find prepared [Postman Collection](/assets/ShelfyCollection.postman_collectionv2.json) of requests, in assets [folder.](/assets)

### Solution structure
+ Shelfy.Api - Actual HTTP API - Does reference to infrastructure project.
+ Shelfy.Infrastructure - Contains application services, repositories, dtos, mongo convetions, automapper configuration. Does reference to core project. 
+ Shelfy.Core - Core of our application, contains domain models, and repositories interfaces. <br> Does not reference any other project.
+ Shelfy.Tests - Unit tests.
+ Shelfy.EndToEnd - Integration tests.

### Technology stack
+ [.NET Core](https://dotnet.microsoft.com/) - an open source & cross-platform framework for building applications using C# language.
+ [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.2) - is a cross-platform, high-performance, open-source framework for building modern, cloud-based, Internet-connected applications.



