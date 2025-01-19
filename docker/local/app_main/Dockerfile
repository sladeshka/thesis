FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
COPY ./app/SalesVentilationEquipment /app
WORKDIR /app
COPY ["SalesVentilationEquipment.Server/SalesVentilationEquipment.Server.csproj", "SalesVentilationEquipment.Server/"]
RUN dotnet restore "./SalesVentilationEquipment.Server/SalesVentilationEquipment.Server.csproj"
COPY . .
WORKDIR "/app/SalesVentilationEquipment.Server"
RUN dotnet build "./SalesVentilationEquipment.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SalesVentilationEquipment.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SalesVentilationEquipment.Server.dll"]
