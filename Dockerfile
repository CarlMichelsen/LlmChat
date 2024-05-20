FROM node AS frontend

WORKDIR /src

COPY ["Frontend", "."]

RUN npm install

RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["llmchat.sln", "."]

RUN mkdir -p CounterSearch/wwwroot

COPY --from=frontend ["src/dist/", "App/wwwroot/"]

COPY . .

RUN dotnet restore

RUN dotnet test

RUN dotnet build "./App" -c Release

FROM build AS publish

RUN dotnet publish "./App" -c Release --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "App.dll"]