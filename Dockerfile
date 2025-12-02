# Etapa 1: Construcción (Build)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Copiar la solución y los proyectos
# Como el contexto será la carpeta "Aventour", copiamos directamente desde ahí
COPY ["Aventour.sln", "."]
COPY ["Aventour.WebAPI/Aventour.WebAPI.csproj", "Aventour.WebAPI/"]
COPY ["Aventour.Application/Aventour.Application.csproj", "Aventour.Application/"]
COPY ["Aventour.Domain/Aventour.Domain.csproj", "Aventour.Domain/"]
COPY ["Aventour.Infrastructure/Aventour.Infrastructure.csproj", "Aventour.Infrastructure/"]

# 2. Restaurar dependencias
RUN dotnet restore "Aventour.sln"

# 3. Copiar todo el código fuente restante
COPY . .

# 4. Publicar la aplicación
WORKDIR "/src/Aventour.WebAPI"
RUN dotnet publish "Aventour.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Aventour.WebAPI.dll"]