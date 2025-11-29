using Aventour.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Aventour.Domain.Enums;
using Aventour.Domain.Models;
using Npgsql;

namespace Aventour.Infrastructure.Persistence.Context;

public partial class AventourDbContext : DbContext
{
    public AventourDbContext()
    {
    }

    public AventourDbContext(DbContextOptions<AventourDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AgenciasGuia> AgenciasGuias { get; set; }
    public virtual DbSet<DestinosTuristico> DestinosTuristicos { get; set; }
    public virtual DbSet<DetallePackDestino> DetallePackDestinos { get; set; }
    public virtual DbSet<DetalleRuta> DetalleRutas { get; set; }
    public virtual DbSet<Favorito> Favoritos { get; set; }
    public virtual DbSet<HotelesRestaurante> HotelesRestaurantes { get; set; }
    public virtual DbSet<PacksRutasAgencium> PacksRutasAgencia { get; set; }
    public virtual DbSet<Resena> Resenas { get; set; }
    public virtual DbSet<RutasPersonalizada> RutasPersonalizadas { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. CONFIGURACIÓN DE ENUMS (Solo los que NO cambiaste a VARCHAR)
        // Mantenemos Agencia y Hotel como Enum nativo si no los has migrado aún.
        modelBuilder.HasPostgresEnum<TipoAgenciaGuia>("tipo_agencia_guia");
        modelBuilder.HasPostgresEnum("tipo_hotel_rest", new[] { "Hotel", "Restaurante" });

        // Nota: Se han eliminado los HasPostgresEnum de Favorito y Resena 
        // porque ahora se manejan como texto (string).

        // 2. CONFIGURACIÓN DE ENTIDADES
        modelBuilder.Entity<AgenciasGuia>(entity =>
        {
            entity.HasKey(e => e.IdAgencia).HasName("agencias_guias_pkey");
            entity.ToTable("agencias_guias");
            entity.Property(e => e.IdAgencia).HasColumnName("id_agencia");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Email).HasMaxLength(100).HasColumnName("email");
            entity.Property(e => e.Nombre).HasMaxLength(255).HasColumnName("nombre");
            entity.Property(e => e.PuntuacionMedia).HasPrecision(2, 1).HasDefaultValueSql("0.0").HasColumnName("puntuacion_media");
            entity.Property(e => e.Validado).HasDefaultValue(false).HasColumnName("validado");
            entity.Property(e => e.WhatsappContacto).HasMaxLength(20).HasColumnName("whatsapp_contacto");
            // Nota: Aquí se asume que 'Tipo' sigue siendo Enum en BD. 
            // Si cambias la BD, agrega .HasConversion<string>() aquí también.
        });

        modelBuilder.Entity<DestinosTuristico>(entity =>
        {
            entity.HasKey(e => e.IdDestino).HasName("destinos_turisticos_pkey");
            entity.ToTable("destinos_turisticos");
            entity.HasIndex(e => e.Nombre, "destinos_turisticos_nombre_key").IsUnique();
            entity.Property(e => e.IdDestino).HasColumnName("id_destino");
            entity.Property(e => e.CostoEntrada).HasPrecision(10, 2).HasDefaultValueSql("0.00").HasColumnName("costo_entrada");
            entity.Property(e => e.DescripcionBreve).HasColumnName("descripcion_breve");
            entity.Property(e => e.DescripcionCompleta).HasColumnName("descripcion_completa");
            entity.Property(e => e.HorarioAtencion).HasMaxLength(255).HasColumnName("horario_atencion");
            entity.Property(e => e.Latitud).HasPrecision(10, 8).HasColumnName("latitud");
            entity.Property(e => e.Longitud).HasPrecision(11, 8).HasColumnName("longitud");
            entity.Property(e => e.Nombre).HasMaxLength(255).HasColumnName("nombre");
            entity.Property(e => e.PuntuacionMedia).HasPrecision(2, 1).HasDefaultValueSql("0.0").HasColumnName("puntuacion_media");
            entity.Property(e => e.Tipo).HasMaxLength(50).HasColumnName("tipo");
            entity.Property(e => e.UrlFotoPrincipal).HasMaxLength(255).HasColumnName("url_foto_principal");
        });

        modelBuilder.Entity<DetallePackDestino>(entity =>
        {
            entity.HasKey(e => e.IdDetallePack).HasName("detalle_pack_destinos_pkey");
            entity.ToTable("detalle_pack_destinos");
            entity.HasIndex(e => new { e.IdPack, e.OrdenParada }, "detalle_pack_destinos_id_pack_orden_parada_key").IsUnique();
            entity.Property(e => e.IdDetallePack).HasColumnName("id_detalle_pack");
            entity.Property(e => e.IdDestino).HasColumnName("id_destino");
            entity.Property(e => e.IdPack).HasColumnName("id_pack");
            entity.Property(e => e.NotasDia).HasMaxLength(255).HasColumnName("notas_dia");
            entity.Property(e => e.OrdenParada).HasColumnName("orden_parada");
            entity.HasOne(d => d.IdDestinoNavigation).WithMany(p => p.DetallePackDestinos).HasForeignKey(d => d.IdDestino).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("detalle_pack_destinos_id_destino_fkey");
            entity.HasOne(d => d.IdPackNavigation).WithMany(p => p.DetallePackDestinos).HasForeignKey(d => d.IdPack).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("detalle_pack_destinos_id_pack_fkey");
        });

        modelBuilder.Entity<DetalleRuta>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("detalle_rutas_pkey");
            entity.ToTable("detalle_rutas");
            entity.HasIndex(e => new { e.IdRuta, e.OrdenParada }, "detalle_rutas_id_ruta_orden_parada_key").IsUnique();
            entity.Property(e => e.IdDetalle).HasColumnName("id_detalle");
            entity.Property(e => e.IdDestino).HasColumnName("id_destino");
            entity.Property(e => e.IdRuta).HasColumnName("id_ruta");
            entity.Property(e => e.Notas).HasColumnName("notas");
            entity.Property(e => e.OrdenParada).HasColumnName("orden_parada");
            entity.HasOne(d => d.IdDestinoNavigation).WithMany(p => p.DetalleRuta).HasForeignKey(d => d.IdDestino).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("detalle_rutas_id_destino_fkey");
            entity.HasOne(d => d.IdRutaNavigation).WithMany(p => p.DetalleRuta).HasForeignKey(d => d.IdRuta).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("detalle_rutas_id_ruta_fkey");
        });

        modelBuilder.Entity<Favorito>(entity =>
        {
            // Clave primaria compuesta
            entity.HasKey(e => new { e.IdUsuario, e.IdEntidad, e.TipoEntidad })
                .HasName("favoritos_pkey");
        
            entity.ToTable("favoritos");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdEntidad).HasColumnName("id_entidad");

            // --- CORRECCIÓN IMPORTANTE ---
            // Se mapea como string para que PostgreSQL reciba texto (VARCHAR)
            entity.Property(e => e.TipoEntidad)
                .HasColumnName("tipo_entidad")
                .HasConversion<string>(); // Enum C# <-> String BD

            entity.Property(e => e.FechaGuardado)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_guardado");
              
            entity.HasOne(d => d.IdUsuarioNavigation).WithMany()
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favoritos_id_usuario_fkey");
        });

        modelBuilder.Entity<HotelesRestaurante>(entity =>
        {
            entity.HasKey(e => e.IdLugar).HasName("hoteles_restaurantes_pkey");
            entity.ToTable("hoteles_restaurantes");

            entity.Property(e => e.IdLugar).HasColumnName("id_lugar");
            entity.Property(e => e.Direccion).HasMaxLength(255).HasColumnName("direccion");
            entity.Property(e => e.Latitud).HasPrecision(10, 8).HasColumnName("latitud");
            entity.Property(e => e.Longitud).HasPrecision(11, 8).HasColumnName("longitud");
            entity.Property(e => e.Nombre).HasMaxLength(255).HasColumnName("nombre");
            entity.Property(e => e.PuntuacionMedia).HasPrecision(2, 1).HasDefaultValueSql("0.0").HasColumnName("puntuacion_media");
            // Si también cambiaste la tabla hoteles, agrega .HasConversion<string>() a la propiedad 'Tipo'
        });

        modelBuilder.Entity<PacksRutasAgencium>(entity =>
        {
            entity.HasKey(e => e.IdPack).HasName("packs_rutas_agencia_pkey");
            entity.ToTable("packs_rutas_agencia");
            entity.Property(e => e.IdPack).HasColumnName("id_pack");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.DuracionDias).HasDefaultValue(1).HasColumnName("duracion_dias");
            entity.Property(e => e.IdAgencia).HasColumnName("id_agencia");
            entity.Property(e => e.NombrePack).HasMaxLength(255).HasColumnName("nombre_pack");
            entity.Property(e => e.PrecioBase).HasPrecision(10, 2).HasColumnName("precio_base");
            entity.HasOne(d => d.IdAgenciaNavigation).WithMany(p => p.PacksRutasAgencia).HasForeignKey(d => d.IdAgencia).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("packs_rutas_agencia_id_agencia_fkey");
            entity.HasMany(d => d.IdDestinos).WithMany(p => p.IdPacks)
                .UsingEntity<Dictionary<string, object>>("DestinosAgencium",
                    r => r.HasOne<DestinosTuristico>().WithMany().HasForeignKey("IdDestino").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("destinos_agencia_id_destino_fkey"),
                    l => l.HasOne<PacksRutasAgencium>().WithMany().HasForeignKey("IdPack").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("destinos_agencia_id_pack_fkey"),
                    j =>
                    {
                        j.HasKey("IdPack", "IdDestino").HasName("destinos_agencia_pkey");
                        j.ToTable("destinos_agencia");
                        j.IndexerProperty<int>("IdPack").HasColumnName("id_pack");
                        j.IndexerProperty<int>("IdDestino").HasColumnName("id_destino");
                    });
        });

        modelBuilder.Entity<Resena>(entity =>
        {
            entity.HasKey(e => e.IdResena).HasName("resenas_pkey");
            entity.ToTable("resenas");

            entity.Property(e => e.IdResena).HasColumnName("id_resena");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdEntidad).HasColumnName("id_entidad");
            
            // --- AGREGA ESTA CONFIGURACIÓN ---
            entity.Property(e => e.TipoEntidad)
                .HasColumnName("tipo_entidad")
                .HasConversion<string>(); // Esto convierte Enum <-> String automáticamente
            // ---------------------------------

            entity.Property(e => e.Puntuacion).HasColumnName("puntuacion");
            entity.Property(e => e.Comentario).HasColumnName("comentario");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_creacion");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Resenas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("resenas_id_usuario_fkey");
        });

        modelBuilder.Entity<RutasPersonalizada>(entity =>
        {
            entity.HasKey(e => e.IdRuta).HasName("rutas_personalizadas_pkey");
            entity.ToTable("rutas_personalizadas");

            entity.Property(e => e.IdRuta).HasColumnName("id_ruta");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IsPublica).HasDefaultValue(false).HasColumnName("is_publica");
            entity.Property(e => e.NombreRuta).HasMaxLength(255).HasColumnName("nombre_ruta");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.RutasPersonalizada)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rutas_personalizadas_id_usuario_fkey");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("usuarios_pkey");
            entity.ToTable("usuarios");
            entity.HasIndex(e => e.Email, "usuarios_email_key").IsUnique();
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario").UseIdentityByDefaultColumn();
            entity.Property(e => e.Nombres).HasMaxLength(100).HasColumnName("nombres");
            entity.Property(e => e.Apellidos).HasMaxLength(100).HasColumnName("apellidos");
            entity.Property(e => e.Email).HasMaxLength(100).HasColumnName("email");
            entity.Property(e => e.PasswordHash).HasMaxLength(255).HasColumnName("password_hash");
            entity.Property(e => e.Edad).HasColumnName("edad");
            entity.Property(e => e.EstadoCivil).HasMaxLength(50).HasColumnName("estado_civil");
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("now()").HasColumnType("timestamp without time zone").HasColumnName("fecha_registro");
            entity.Property(e => e.EsAdministrador).HasDefaultValue(false).HasColumnName("es_administrador");
            entity.Property(e => e.TokenConfirmacion).HasMaxLength(255).HasColumnName("token_confirmacion");
            entity.Property(e => e.SesionActiva).HasDefaultValue(false).HasColumnName("sesion_activa");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}