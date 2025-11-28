using System.Text;
using Aventour.Application.DTOs;
using Aventour.Application.Services;
using Aventour.Application.Services.Destinos;
using Aventour.Application.UseCases.Destinos;
using Aventour.Application.UseCases.Favoritos;
using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Authentication;
using Aventour.Infrastructure.Persistence.Context;
using Aventour.Infrastructure.Persistence.Repositories;
using Aventour.Infrastructure.Persistence.UnitOfWork; // Asumo que IUnitOfWork está implementado aquí
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Aventour.Application.UseCases.Favoritos; // Asegúrate de tener este using para la interfaz
using Aventour.Application.Services.Favoritos;
using Aventour.Infrastructure.Repositories; // Asumo que la implementación está aquí

var builder = WebApplication.CreateBuilder(args);

// ===============================================
// 1. Agregar conexión a PostgreSQL usando appsettings.json
// ===============================================
builder.Services.AddDbContext<AventourDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AventourConnection"))
);

// Necesario para PostgreSQL (fecha y enums)
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
// 3. INYECCIÓN DE DEPENDENCIAS HEXAGONAL 🧱
// =============================================================

// Repositorios y Unidad de Trabajo (Infraestructura / Adaptadores de Salida)
// Se elimina la línea duplicada IUnitOfWork.

// 1. Repositorio (Infraestructura)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Registro central de UoW
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IDestinoRepository, DestinoRepository>();
builder.Services.AddScoped<IFavoritoRepository, FavoritoRepository>(); // Repositorio de Favoritos

// Servicios / Casos de Uso (Aplicación / Dominio)
// Servicios de Usuarios
builder.Services.AddScoped<UsuarioService>(); // Si no tiene interfaz
builder.Services.AddScoped<JwtTokenGenerator>();

// Servicios de Destinos
builder.Services.AddScoped<IDestinoService, DestinoService>();
builder.Services.AddScoped<IGestionarDestinosUseCase, GestionarDestinosUseCase>();
builder.Services.AddScoped<IConsultarDestinosUseCase, ConsultarDestinosUseCase>();

// Servicios de Favoritos (Tanto la interfaz como la implementación)
// Registro de AutoMapper: Busca todos los perfiles (como FavoritoMappingProfile) en el ensamblado de Aventour.Application
// Usamos el ensamblado de una clase conocida en la capa Application, como FavoritoDto.
builder.Services.AddAutoMapper(typeof(FavoritoDto).Assembly);

// Registro de los Servicios/Casos de Uso
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

builder.Services.AddControllers();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();