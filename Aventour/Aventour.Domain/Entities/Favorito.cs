using Aventour.Domain.Enums;
using System;

namespace Aventour.Domain.Entities;

public class Favorito
{
    // Clave Compuesta 1: ID del Usuario que guardó el favorito
    public int IdUsuario { get; set; } 
    
    // Clave Compuesta 2: ID de la entidad (Destino, Hotel, Agencia, etc.)
    public int IdEntidad { get; set; } 
    
    // Clave Compuesta 3: Tipo de la entidad (define si IdEntidad es un Destino, Hotel, etc.)
    public TipoFavorito TipoEntidad { get; set; }
    
    public DateTime FechaGuardado { get; set; }

    // Propiedad de Navegación (Relación con la tabla Usuarios)
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}