# Imagen base de .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copiar archivos de solución y proyecto
COPY *.sln .
COPY HotelAplication/*.csproj ./HotelAplication/

# Restaurar dependencias
RUN dotnet restore

# Copiar todo el código y compilar
COPY . .
WORKDIR /app/HotelAplication
RUN dotnet publish -c Release -o /app/publish

# Imagen final para ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "HotelAplication.dll"]
