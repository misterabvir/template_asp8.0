#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AuthenticationService/AuthenticationService.AuthenticationService.Presentation/AuthenticationService.AuthenticationService.Presentation.csproj", "src/AuthenticationService/AuthenticationService.AuthenticationService.Presentation/"]
COPY ["src/AuthenticationService/AuthenticationService.Application/AuthenticationService.AuthenticationService.Application.csproj", "src/AuthenticationService/AuthenticationService.AuthenticationService.Application/"]
COPY ["src/AuthenticationService/AuthenticationService.Domain/AuthenticationService.Domain.csproj", "src/AuthenticationService/AuthenticationService.AuthenticationService.Domain/"]
COPY ["src/Shared/Shared.csproj", "src/Shared/"]
COPY ["src/AuthenticationService/AuthenticationService.Infrastructure/AuthenticationService.Infrastructure.csproj", "src/AuthenticationService/AuthenticationService.Infrastructure/"]
RUN dotnet restore "./src/AuthenticationService/AuthenticationService.Presentation/AuthenticationService.Presentation.csproj"
COPY . .
WORKDIR "/src/src/AuthenticationService/AuthenticationService.AuthenticationService.Presentation"
RUN dotnet build "./AuthenticationService.AuthenticationService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuthenticationService.AuthenticationService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthenticationService.AuthenticationService.Presentation.dll"]