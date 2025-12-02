using System.Text;
using System.Text.Json.Serialization;
using Aventour.Application.DTOs;
using Aventour.Application.Repositories;
using Aventour.Application.Services;
using Aventour.Application.Services.Agencias; // UsuarioService
using Aventour.Application.Services.Destinos;
using Aventour.Application.Services.Favoritos;
using Aventour.Application.Services.Packs;
using Aventour.Application.Services.Resenas;
using Aventour.Application.UseCases.Destinos;
 
using Aventour.Domain.Enums; // <--- Necesario para tus Enums
using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Authentication;
using Aventour.Infrastructure.Persistence.Context;
using Aventour.Infrastructure.Persistence.Repositories;
using Aventour.Infrastructure.Persistence.UnitOfWork;
using Aventour.Infrastructure.Repositories;
using Aventour.Infrastructure.Security; // JwtTokenGenerator
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql; // <--- Necesario para NpgsqlDataSourceBuilder

var builder = WebApplication.CreateBuilder(args);

// ===============================================
// 1. BASE DE DATOS: Configuración Correcta con Enums
// ===============================================

// A. Obtener cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("AventourConnection");

// B. Crear el DataSourceBuilder (Esta es la forma moderna de Npgsql)
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

// C. Mapear tus Enums AQUÍ. Deben coincidir exactamente con los nombres en Postgres.
// Estos Enum son mapeados para garantizar que se gestionen correctamente las enumeraciones en la base de datos.
// Descomenta las siguientes líneas si las necesitas:
// dataSourceBuilder.MapEnum<TipoFavorito>("public.tipo_favorito");
// dataSourceBuilder.MapEnum<TipoAgenciaGuia>("public.tipo_agencia_guia");
dataSourceBuilder.MapEnum<TipoResena>("public.tipo_resena");
dataSourceBuilder.MapEnum<TipoHotelRest>("public.tipo_hotel_rest");

// Registra el repositorio de Agencias para ser utilizado en los servicios
builder.Services.AddScoped<IAgenciaRepository, AgenciaRepository>();

// D. Construir el DataSource
var dataSource = dataSourceBuilder.Build();

// Registra servicios relacionados con Reseñas
builder.Services.AddScoped<IResenaRepository, ResenaRepository>();
builder.Services.AddScoped<IResenaService, ResenaService>();

// E. Inyectar el DbContext usando el DataSource configurado para interactuar con PostgreSQL
builder.Services.AddDbContext<AventourDbContext>(options =>
    options.UseNpgsql(dataSource)
);

// (Opcional) Switch legacy para timestamps en PostgreSQL
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// =============================================================
// 2. SWAGGER + JWT SEGURIDAD
// =============================================================

builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger para la documentación de la API
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Aventour API", 
        Version = "v1",
        Description = "API para gestionar servicios turísticos en Arequipa, Perú"
    });

    // Definición de la seguridad con JWT Bearer
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http, 
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header, 
        Description = "Ingresa tu token JWT aquí usando el formato: Bearer {token}"
    });

    // Requerimiento de seguridad en cada endpoint para la autenticación Bearer
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme 
            {
                Reference = new OpenApiReference 
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// =============================================================
// 3. INYECCIÓN DE DEPENDENCIAS
// =============================================================

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IDestinoRepository, DestinoRepository>();
builder.Services.AddScoped<IFavoritoRepository, FavoritoRepository>();

// Servicios de usuario y autenticación
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<JwtTokenGenerator>();

// Configuración de servicios de Destinos y Casos de Uso
builder.Services.AddScoped<IDestinoService, DestinoService>();
builder.Services.AddScoped<IGestionarDestinosUseCase, GestionarDestinosUseCase>();
builder.Services.AddScoped<IConsultarDestinosUseCase, ConsultarDestinosUseCase>();

// Favoritos (AutoMapper + Servicio)
builder.Services.AddAutoMapper(typeof(FavoritoDto).Assembly);
builder.Services.AddScoped<IFavoritoService, FavoritoService>();

// Registros de servicios y repositorios relacionados con Agencias, Rutas, Packs, Hoteles y Restaurantes
builder.Services.AddScoped<IAgenciaRepository, AgenciaRepository>();
builder.Services.AddScoped<IAgenciaService, AgenciaService>();
builder.Services.AddScoped<IRutaPersonalizadaRepository, RutaPersonalizadaRepository>();
builder.Services.AddScoped<RutaPersonalizadaService>();
builder.Services.AddScoped<IPackRutaRepository, PackRutaRepository>();
builder.Services.AddScoped<PackRutaService>();
builder.Services.AddScoped<IHotelRestauranteRepository, HotelRestauranteRepository>();
builder.Services.AddScoped<HotelRestauranteService>();

// Servicios para la creación de reportes
builder.Services.AddScoped<ReporteService>();

// Configuración para trabajar con JWT y la validación de tokens
System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// =============================================================
// 4. AUTENTICACIÓN JWT
// =============================================================

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });

// Configuración de controladores y opciones de JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Esto permite que envíes "Destino" (string) en el JSON en lugar de 0 o 1 para los Enums
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

// =============================================================
// 5. MIDDLEWARES
// =============================================================

// Configuración de Swagger UI solo en el entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware para manejar la expiración de tokens (esto debe manejar la expiración correctamente)
app.UseMiddleware<Aventour.WebAPI.Middleware.TokenExpirationMiddleware>();

// Configuración de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Rutas de los controladores
app.MapControllers();

// Ejecuta la aplicación
app.Run();
