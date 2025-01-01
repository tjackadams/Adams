#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Nexus.Hub/Nexus.Portal/Nexus.Portal.csproj", "Nexus.Hub/Nexus.Portal/"]
COPY ["Nexus.Hub/Nexus.Todo.Client/Nexus.Todo.Client.csproj", "Nexus.Hub/Nexus.Todo.Client/"]
COPY ["Nexus.Hub/Nexus.WeightTracker.Client/Nexus.WeightTracker.Client.csproj", "Nexus.Hub/Nexus.WeightTracker.Client/"]
RUN dotnet restore "Nexus.Hub/Nexus.Portal/Nexus.Portal.csproj"
COPY . .
WORKDIR "/src/Nexus.Hub/Nexus.Portal"
RUN dotnet build "Nexus.Portal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nexus.Portal.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nexus.Portal.dll"]