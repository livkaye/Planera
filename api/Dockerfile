﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5065

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["api/Planera.csproj", "api/"]
RUN dotnet restore "api/Planera.csproj"
COPY . .
WORKDIR /src/api
RUN dotnet build "Planera.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Planera.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app/data
COPY --from=publish /app/publish /app
COPY --from=publish /app/publish/appsettings.json /app/data
RUN echo "cp -n ../appsettings.json .; dotnet ../Planera.dll" >> ../start.sh
ENTRYPOINT ["sh", "../start.sh"]
