FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7170
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:7170

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/MoneyMe.API/MoneyMe.API.csproj", "MoneyMe.API/"]
COPY ["src/MeoneyMe.Application/MeoneyMe.Application.csproj", "MeoneyMe.Application/"]
COPY ["src/MoneyMe.Domain/MoneyMe.Domain.csproj", "MoneyMe.Domain/"]
COPY ["src/MoneyMe.Contracts/MoneyMe.Contracts.csproj", "MoneyMe.Contracts/"]
COPY ["src/MeoneyMe.Infrastructure/MeoneyMe.Infrastructure.csproj", "MeoneyMe.Infrastructure/"]

RUN dotnet restore "MoneyMe.API/MoneyMe.API.csproj"

COPY src/ .

WORKDIR "/src/MoneyMe.API"

RUN dotnet build "MoneyMe.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MoneyMe.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "MoneyMe.API.dll"]
