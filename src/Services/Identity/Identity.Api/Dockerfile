FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

# Solution
COPY ["Adams.sln", "Adams.sln"]

# Services
COPY ["Services/Identity/Identity.Api/Identity.Api.csproj", "Services/Identity/Identity.Api/Identity.Api.csproj"]

# Web
COPY ["Web/WebStatus/WebStatus.csproj", "Web/WebStatus/WebStatus.csproj"]
COPY ["Web/WebBlazor/Server/WebBlazor.Server.csproj", "Web/WebBlazor/Server/"]
COPY ["Web/WebBlazor/Shared/WebBlazor.Shared.csproj", "Web/WebBlazor/Shared/"]
COPY ["Web/WebBlazor/Client/WebBlazor.Client.csproj", "Web/WebBlazor/Client/"]

# Shared
COPY ["Shared/Adams.AspNetCore.Extensions/Adams.AspNetCore.Extensions.csproj", "Shared/Adams.AspNetCore.Extensions/Adams.AspNetCore.Extensions.csproj"]
COPY ["Shared/Adams.IdentityServer.AzureKeyVault/Adams.IdentityServer.AzureKeyVault.csproj", "Shared/Adams.IdentityServer.AzureKeyVault/Adams.IdentityServer.AzureKeyVault.csproj"]

RUN dotnet restore "Adams.sln" -r linux-arm


COPY . .
WORKDIR "/src"
RUN dotnet publish "Services/Identity/Identity.Api/Identity.Api.csproj" -c Release -o /app -r linux-arm

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]