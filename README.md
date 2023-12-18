# NetCoreMinimalApi

This solution combines the tutorial [Create a minimal API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio-code) with article [Create a web API with ASP.NET Core and MongoDB](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-8.0&tabs=visual-studio-code) in order to implement an example of minimal API using MongoDb as database.


## Prerequisites
[Visual Studio 2022 (latest version)](https://visualstudio.microsoft.com/vs/#download) or [Visual Studio Code](https://code.visualstudio.com/download) with [C# Extension (latest version)](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) and [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

[MongoDB 6.0.5 or later](https://docs.mongodb.com/manual/tutorial/install-mongodb-on-windows/)

[MongoDB Shell](https://www.mongodb.com/docs/mongodb-shell/install/)

## Containers

The components of the project can run in containers. The project contains a Dockerfile and a docker-compose file to orchestrate the front-end, back-end and MongoDb repository. The Mongo image was pulled from [Docker Hub](https://hub.docker.com) and docker-compose uses a volume for MongoDb to be created with `docker volume create mongodata` command.

The project supports both Visual Studio + Docker Desktop on Windows and VS Code on Linux. For Linux it is needed to install docker and docker-compose using the package manager of chosen distro.
