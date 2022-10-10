#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG TF_BUILD

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["Nexus.Hub/Nexus.Todo.Api/Nexus.Todo.Api.csproj", "Nexus.Hub/Nexus.Todo.Api/"]
RUN dotnet restore "Nexus.Hub/Nexus.Todo.Api/Nexus.Todo.Api.csproj"
COPY . .
WORKDIR "/src/Nexus.Hub/Nexus.Todo.Api"
RUN dotnet build "Nexus.Todo.Api.csproj" -c Release -o /app/build /p:TF_BUILD=$TF_BUILD

FROM build AS publish
RUN dotnet publish "Nexus.Todo.Api.csproj" -c Release -o /app/publish /p:TF_BUILD=$TF_BUILD

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nexus.Todo.Api.dll"]