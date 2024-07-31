#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/MailService/MailService.AuthenticationService.Presentation/MailService.AuthenticationService.Presentation.csproj", "src/MailService/MailService.AuthenticationService.Presentation/"]
COPY ["src/MailService/MailService.AuthenticationService.Application/MailService.AuthenticationService.Application.csproj", "src/MailService.AuthenticationService.Application/MailService.AuthenticationService.Application/"]
COPY ["src/MailService/MailService.Infrastructure/MailService.Infrastructure.csproj", "src/MailService.Infrastructure/MailService.Infrastructure/"]
COPY ["src/MailService/MailService.AuthenticationService.Domain/MailService.AuthenticationService.Domain.csproj", "src/MailService.AuthenticationService.Domain/MailService.AuthenticationService.Domain/"]
COPY ["src/Shared/Shared.csproj", "src/Shared/"]
RUN dotnet restore "./src/MailService/MailService.AuthenticationService.Presentation/MailService.AuthenticationService.Presentation.csproj"
COPY . .
WORKDIR "/src/src/MailService/MailService.AuthenticationService.Presentation"
RUN dotnet build "./MailService.AuthenticationService.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./MailService.AuthenticationService.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MailService.AuthenticationService.Presentation.dll"]