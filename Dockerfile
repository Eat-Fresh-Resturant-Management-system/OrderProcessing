﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5100

ENV ASPNETCORE_URLS=http://+:5126

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["OrderProcessing/OrderProcessing.csproj", "OrderProcessing/"]
RUN dotnet restore "OrderProcessing/OrderProcessing.csproj"


COPY . .
WORKDIR "/src/OrderProcessing" 
RUN dotnet build "OrderProcessing.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "OrderProcessing.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderProcessing.dll"]
