#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AuthenticationService/Presentation/Presentation.csproj", "src/AuthenticationService/Presentation/"]
COPY ["src/AuthenticationService/Application/Application.csproj", "src/AuthenticationService/Application/"]
COPY ["src/AuthenticationService/Domain/Domain.csproj", "src/AuthenticationService/Domain/"]
COPY ["src/Shared/Shared.csproj", "src/Shared/"]
COPY ["src/AuthenticationService/Infrastructure/Infrastructure.csproj", "src/AuthenticationService/Infrastructure/"]
RUN dotnet restore "./src/AuthenticationService/Presentation/Presentation.csproj"
COPY . .
WORKDIR "/src/src/AuthenticationService/Presentation"
RUN dotnet build "./Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.dll"]