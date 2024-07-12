FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app

COPY API/ ./API/
COPY CommandCentralAPI_Prod.sln ./

RUN dotnet restore ./API/API.csproj
RUN dotnet publish ./API/API.csproj -c $BUILD_CONFIGURATION -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# http
EXPOSE 5067 
# https
EXPOSE 7199
ENTRYPOINT [ "dotnet", "API.dll" ]