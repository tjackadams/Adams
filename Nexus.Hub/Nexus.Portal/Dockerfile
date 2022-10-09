#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Nexus.Hub/Nexus.Portal/Nexus.Portal.csproj", "Nexus.Hub/Nexus.Portal/"]
RUN dotnet restore "Nexus.Hub/Nexus.Portal/Nexus.Portal.csproj"
COPY . .
WORKDIR "/src/Nexus.Hub/Nexus.Portal"
RUN dotnet build "Nexus.Portal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nexus.Portal.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nexus.Portal.dll"]