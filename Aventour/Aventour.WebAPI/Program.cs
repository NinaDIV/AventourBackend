using System.Text;
using Aventour.Application.Interfaces;
using Aventour.Application.Services;
using Aventour.Application.Services.Destinos;
using Aventour.Application.Services.Favoritos;
using Aventour.Application.UseCases.Destinos;
using Aventour.Domain.Enums; // IMPORTANTE: Agregar este using
using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Authentication;
using Aventour.Infrastructure.Persistence.Context;
using Aventour.Infrastructure.Persistence.Repositories;
using Aventour.Infrastructure.Persistence.UnitOfWork;
using Aventour.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
 

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

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// =============================================================
// 3. INYECCIÓN DE DEPENDENCIAS HEXAGONAL
// =============================================================
// Usuarios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<JwtTokenGenerator>();

// Destinos
builder.Services.AddScoped<IDestinoRepository, DestinoRepository>();
builder.Services.AddScoped<IGestionarDestinosUseCase, GestionarDestinosUseCase>();
builder.Services.AddScoped<IConsultarDestinosUseCase, ConsultarDestinosUseCase>();
builder.Services.AddScoped<IDestinoService, DestinoService>();

// 1. Repositorio (Infraestructura)
// Asocia la interfaz con su implementación concreta para el acceso a datos.
 
// 1. Repositorios (Adaptadores de Salida)
builder.Services.AddScoped<IFavoritoRepository, FavoritoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<GestorFavoritosService>();
// 2. Servicio/Lógica de Negocio (Aplicación)
// Asocia la interfaz con su implementación de la lógica de negocio.
 

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