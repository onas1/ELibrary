#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY . ./
RUN dotnet restore
COPY . ./
WORKDIR /src
RUN dotnet build
# publish
FROM build AS publish
WORKDIR /src
RUN dotnet publish -c Release -o /src/publish
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /src
COPY --from=publish /src/elibrary.db .
COPY --from=publish /src/publish .
# ENTRYPOINT ["dotnet", "CICDDemo.dll"]
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ELibrary.MVC.dll