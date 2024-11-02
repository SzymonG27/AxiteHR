#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

USER root
COPY AxiteHR.Services.ApplicationAPI/Certs/axitehr.services.applicationapi.crt /usr/local/share/ca-certificates/axitehr.services.applicationapi.crt
COPY AxiteHr.Services.CompanyAPI/Certs/axitehr.services.companyapi.crt /usr/local/share/ca-certificates/axitehr.services.companyapi.crt
RUN update-ca-certificates

USER app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AxiteHR.GatewaySol/AxiteHR.GatewaySol.csproj", "AxiteHR.GatewaySol/"]
RUN dotnet restore "./AxiteHR.GatewaySol/AxiteHR.GatewaySol.csproj"
COPY . .
WORKDIR "/src/AxiteHR.GatewaySol"
RUN dotnet build "./AxiteHR.GatewaySol.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AxiteHR.GatewaySol.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AxiteHR.GatewaySol.dll"]