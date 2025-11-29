using System.Text;
using System.Text.Json.Serialization;
using Aventour.Application.DTOs;
using Aventour.Application.Services; // UsuarioService
using Aventour.Application.Services.Destinos;
using Aventour.Application.Services.Favoritos;
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
// dataSourceBuilder.MapEnum<TipoFavorito>("public.tipo_favorito");
// dataSourceBuilder.MapEnum<TipoAgenciaGuia>("public.tipo_agencia_guia");
dataSourceBuilder.MapEnum<TipoResena>("public.tipo_resena");
dataSourceBuilder.MapEnum<TipoHotelRest>("public.tipo_hotel_rest");


builder.Services.AddScoped<IAgenciaRepository, AgenciaRepository>();
// D. Construir el DataSource
var dataSource = dataSourceBuilder.Build();

//Reseñas
builder.Services.AddScoped<IResenaRepository, ResenaRepository>();
builder.Services.AddScoped<IResenaService, ResenaService>();

// E. Inyectar el DbContext usando el dataSource configurado
builder.Services.AddDbContext<AventourDbContext>(options =>
    options.UseNpgsql(dataSource)
);

// (Opcional) Switch legacy para timestamps, aunque con el mapeo suele ser menos crítico
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// =============================================================
// 2. SWAGGER + JWT SEGURIDAD
// =============================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Aventour API", 
        Version = "v1" 
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http, 
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header, 
        Description = "Ingresa tu token JWT aquí usando el formato: Bearer {token}"
    });

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

// --- Repositorios ---
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IDestinoRepository, DestinoRepository>();
builder.Services.AddScoped<IFavoritoRepository, FavoritoRepository>();

// --- Servicios / Casos de Uso ---
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<JwtTokenGenerator>();

// Destinos
builder.Services.AddScoped<IDestinoService, DestinoService>();
builder.Services.AddScoped<IGestionarDestinosUseCase, GestionarDestinosUseCase>();
builder.Services.AddScoped<IConsultarDestinosUseCase, ConsultarDestinosUseCase>();

// Favoritos (AutoMapper + Servicio)
builder.Services.AddAutoMapper(typeof(FavoritoDto).Assembly);
builder.Services.AddScoped<IFavoritoService, FavoritoService>();

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

// Configuración de Controladores y JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Esto permite que envíes "Destino" (string) en el JSON en lugar de 0 o 1
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

// =============================================================
// 5. MIDDLEWARES
// =============================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔹 Middleware para detectar tokens expirados
app.UseMiddleware<Aventour.WebAPI.Middleware.TokenExpirationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();