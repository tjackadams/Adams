#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Nexus.Hub/Nexus.WeightTracker.Api/Nexus.WeightTracker.Api.csproj", "Nexus.Hub/Nexus.WeightTracker.Api/"]
RUN dotnet restore "Nexus.Hub/Nexus.WeightTracker.Api/Nexus.WeightTracker.Api.csproj"
COPY . .
WORKDIR "/src/Nexus.Hub/Nexus.WeightTracker.Api"
RUN dotnet build "Nexus.WeightTracker.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nexus.WeightTracker.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nexus.WeightTracker.Api.dll"]