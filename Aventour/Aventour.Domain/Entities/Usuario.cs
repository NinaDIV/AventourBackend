namespace Aventour.Domain.Entities;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int? Edad { get; set; }

    public string? EstadoCivil { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? EsAdministrador { get; set; }

    public string? TokenConfirmacion { get; set; }

    public bool? SesionActiva { get; set; }

    public virtual ICollection<Resena> Resenas { get; set; } = new List<Resena>();

    public virtual ICollection<RutasPersonalizada> RutasPersonalizada { get; set; } = new List<RutasPersonalizada>();
}
