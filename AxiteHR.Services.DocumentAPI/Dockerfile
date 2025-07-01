#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

USER root

RUN apt-get update && apt-get install -y \
    libnss3 \
    libx11-xcb1 \
    libxcomposite1 \
    libxdamage1 \
    libxrandr2 \
    libgbm1 \
    libasound2 \
    libatk-bridge2.0-0 \
    libgtk-3-0 \
    fonts-liberation \
    xdg-utils \
    libgdk-pixbuf2.0-0 \
    libglib2.0-0 \
    wget \
    unzip \
    && apt-get clean && rm -rf /var/lib/apt/lists/*

USER app

EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AxiteHR.Services.DocumentAPI/AxiteHR.Services.DocumentAPI.csproj", "AxiteHR.Services.DocumentAPI/"]
RUN dotnet restore "./AxiteHR.Services.DocumentAPI/AxiteHR.Services.DocumentAPI.csproj"
COPY . .
WORKDIR "/src/AxiteHR.Services.DocumentAPI"
RUN dotnet build "./AxiteHR.Services.DocumentAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AxiteHR.Services.DocumentAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AxiteHR.Services.DocumentAPI.dll"]