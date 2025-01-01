#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Nexus.Hub/Nexus.Gateway/Nexus.Gateway.csproj", "Nexus.Hub/Nexus.Gateway/"]
RUN dotnet restore "Nexus.Hub/Nexus.Gateway/Nexus.Gateway.csproj"
COPY . .
WORKDIR "/src/Nexus.Hub/Nexus.Gateway"
RUN dotnet build "Nexus.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nexus.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nexus.Gateway.dll"]