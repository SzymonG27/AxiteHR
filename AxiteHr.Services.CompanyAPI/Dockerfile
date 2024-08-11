#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

USER root
COPY AxiteHr.Services.CompanyAPI/Certs/axitehr.services.companyapi.crt /usr/local/share/ca-certificates/axitehr.services.companyapi.crt
COPY AxiteHR.Services.AuthAPI/Certs/axitehr.services.authapi.crt /usr/local/share/ca-certificates/axitehr.services.authapi.crt
RUN update-ca-certificates

USER app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AxiteHr.Services.CompanyAPI/AxiteHr.Services.CompanyAPI.csproj", "AxiteHr.Services.CompanyAPI/"]
COPY ["AxiteHR.GlobalizationResources/AxiteHR.GlobalizationResources.csproj", "AxiteHR.GlobalizationResources/"]
RUN dotnet restore "./AxiteHr.Services.CompanyAPI/AxiteHr.Services.CompanyAPI.csproj"
COPY . .
WORKDIR "/src/AxiteHr.Services.CompanyAPI"
RUN dotnet build "./AxiteHr.Services.CompanyAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AxiteHr.Services.CompanyAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AxiteHr.Services.CompanyAPI.dll"]