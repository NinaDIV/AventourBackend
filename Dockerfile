# Etapa 1: Construcción (Build)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar el archivo de solución y los proyectos para restaurar dependencias
# Ajustamos las rutas basándonos en tu estructura "Aventour/..."
COPY ["Aventour/Aventour.sln", "Aventour/"]
COPY ["Aventour/Aventour.WebAPI/Aventour.WebAPI.csproj", "Aventour/Aventour.WebAPI/"]
COPY ["Aventour/Aventour.Application/Aventour.Application.csproj", "Aventour/Aventour.Application/"]
COPY ["Aventour/Aventour.Domain/Aventour.Domain.csproj", "Aventour/Aventour.Domain/"]
COPY ["Aventour/Aventour.Infrastructure/Aventour.Infrastructure.csproj", "Aventour/Aventour.Infrastructure/"]

# Restaurar dependencias (esto usa la caché de Docker para ser más rápido)
RUN dotnet restore "Aventour/Aventour.sln"

# Copiar el resto del código fuente
COPY . .

# Publicar la aplicación
WORKDIR "/src/Aventour/Aventour.WebAPI"
RUN dotnet publish "Aventour.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime (Imagen final más ligera)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Configurar el puerto que Render espera (8080 es el estándar)
ENV ASPNETCORE_HTTP_PORTS=8080

# Copiar los archivos publicados desde la etapa de construcción
COPY --from=build /app/publish .

# Punto de entrada
ENTRYPOINT ["dotnet", "Aventour.WebAPI.dll"]