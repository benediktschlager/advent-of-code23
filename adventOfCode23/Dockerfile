FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["adventOfCode23/adventOfCode23.csproj", "adventOfCode23/"]
RUN dotnet restore "adventOfCode23/adventOfCode23.csproj"
COPY . .
WORKDIR "/src/adventOfCode23"
RUN dotnet build "adventOfCode23.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "adventOfCode23.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "adventOfCode23.dll"]
