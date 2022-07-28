#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/PasswordManager.API/PasswordManager.API.csproj", "src/PasswordManager.API/"]
COPY ["src/Common/PasswordManager.Logging/PasswordManager.Logging.csproj", "src/Common/PasswordManager.Logging/"]
COPY ["src/Common/PasswordManager.Contracts/PasswordManager.Contracts.csproj", "src/Common/PasswordManager.Contracts/"]
RUN dotnet restore "src/PasswordManager.API/PasswordManager.API.csproj"
COPY . .
WORKDIR "/src/src/PasswordManager.API"
RUN dotnet build "PasswordManager.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PasswordManager.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PasswordManager.API.dll"]