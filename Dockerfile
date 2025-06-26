# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copiamos y restauramos dependencias
COPY *.sln .
COPY HotelAplication/*.csproj ./HotelAplication/
RUN dotnet restore

# Copiar todo el código
COPY . .
WORKDIR /app/HotelAplication
RUN dotnet publish -c Release -o /out

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /out .

EXPOSE 80
ENTRYPOINT ["dotnet", "HotelAplication.dll"]
