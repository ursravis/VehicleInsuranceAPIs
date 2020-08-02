#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
# ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
# ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MetadataService-src/MetadataService.API/MetadataService.API.csproj", "MetadataService-src/MetadataService.API/"]
COPY ["MetadataService-src/MetadataService.Infrastructure/MetadataService.Infrastructure.csproj", "MetadataService-src/MetadataService.Infrastructure/"]
COPY ["MetadataService-src/MetadataService.Core/MetadataService.Core.csproj", "MetadataService-src/MetadataService.Core/"]
COPY ["MetadataService-src/MetadataService.DTO/MetadataService.DTO.csproj", "MetadataService-src/MetadataService.DTO/"]
COPY ["MetadataService-src/MetadataService.SharedKernel/MetadataService.SharedKernel.csproj", "MetadataService-src/MetadataService.SharedKernel/"]
RUN dotnet restore "MetadataService-src/MetadataService.API/MetadataService.API.csproj"
COPY . .
WORKDIR "/src/MetadataService-src/MetadataService.API"
RUN dotnet build "MetadataService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MetadataService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MetadataService.API.dll"]