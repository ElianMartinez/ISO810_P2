# NuGet restore
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY *.sln .
COPY ISO810_P2/*.csproj ISO810_P2/
RUN dotnet restore
COPY . .

# testing
# FROM build AS testing
# WORKDIR /src/Colors.API
# RUN dotnet build
# WORKDIR /src/Colors.UnitTests
# RUN dotnet test

# publish
FROM build AS publish
WORKDIR /src/ISO810_P2
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
# ENTRYPOINT ["dotnet", "ISO810_P2.dll"]
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ISO810_P2.dll