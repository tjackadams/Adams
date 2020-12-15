FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Solution
COPY ["Adams.sln", "Adams.sln"]

# Api Gateways
COPY ["ApiGateways/ApiGw/ApiGw.csproj","ApiGateways/ApiGw/ApiGw.csproj"]

# Services
COPY ["Services/Identity/Identity.Api/Identity.Api.csproj", "Services/Identity/Identity.Api/Identity.Api.csproj"]
COPY ["Services/Smoking/Smoking.Api/Smoking.Api.csproj", "Services/Smoking/Smoking.Api/Smoking.Api.csproj"]
COPY ["Services/Smoking/Smoking.Domain/Smoking.Domain.csproj", "Services/Smoking/Smoking.Domain/Smoking.Domain.csproj"]
COPY ["Services/Smoking/Smoking.Infrastructure/Smoking.Infrastructure.csproj", "Services/Smoking/Smoking.Infrastructure/Smoking.Infrastructure.csproj"]
COPY ["Services/Smoking/Smoking.Migrations/Smoking.Migrations.csproj", "Services/Smoking/Smoking.Migrations/Smoking.Migrations.csproj"]

# Web
COPY ["Web/WebStatus/WebStatus.csproj", "Web/WebStatus/WebStatus.csproj"]
COPY ["Web/WebBlazor/Server/WebBlazor.Server.csproj", "Web/WebBlazor/Server/"]
COPY ["Web/WebBlazor/Shared/WebBlazor.Shared.csproj", "Web/WebBlazor/Shared/"]
COPY ["Web/WebBlazor/Client/WebBlazor.Client.csproj", "Web/WebBlazor/Client/"]

# Shared
COPY ["Shared/Adams.AspNetCore.Extensions/Adams.AspNetCore.Extensions.csproj", "Shared/Adams.AspNetCore.Extensions/Adams.AspNetCore.Extensions.csproj"]
COPY ["Shared/Adams.Core/Adams.Core.csproj", "Shared/Adams.Core/Adams.Core.csproj"]
COPY ["Shared/Adams.Domain/Adams.Domain.csproj", "Shared/Adams.Domain/Adams.Domain.csproj"]
COPY ["Shared/Adams.IdentityServer.AzureKeyVault/Adams.IdentityServer.AzureKeyVault.csproj", "Shared/Adams.IdentityServer.AzureKeyVault/Adams.IdentityServer.AzureKeyVault.csproj"]

RUN dotnet restore "Adams.sln" -r linux-arm

COPY . .
WORKDIR "/src"
RUN dotnet publish "Web/WebBlazor/Server/WebBlazor.Server.csproj" -c Release -o /app -r linux-arm

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebBlazor.Server.dll"]