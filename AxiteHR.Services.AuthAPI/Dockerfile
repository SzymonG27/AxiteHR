#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

USER root
COPY AxiteHR.Services.AuthAPI/Certs/axitehr.services.authapi.crt /usr/local/share/ca-certificates/axitehr.services.authapi.crt
RUN update-ca-certificates

USER app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AxiteHR.Services.AuthAPI/AxiteHR.Services.AuthAPI.csproj", "AxiteHR.Services.AuthAPI/"]
COPY ["AxiteHR.GlobalizationResources/AxiteHR.GlobalizationResources.csproj", "AxiteHR.GlobalizationResources/"]
COPY ["AxiteHR.Integration.MessageBus/AxiteHR.Integration.MessageBus.csproj", "AxiteHR.Integration.MessageBus/"]
RUN dotnet restore "./AxiteHR.Services.AuthAPI/AxiteHR.Services.AuthAPI.csproj"
COPY . .
WORKDIR "/src/AxiteHR.Services.AuthAPI"
RUN dotnet build "./AxiteHR.Services.AuthAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AxiteHR.Services.AuthAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AxiteHR.Services.AuthAPI.dll"]