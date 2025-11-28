using NpgsqlTypes; // Necesitas este using (instala el paquete Npgsql si no te lo reconoce)

namespace Aventour.Domain.Enums
{
    public enum TipoAgenciaGuia
    {
        // El nombre en C# es Agencia, en BD es 'Agencia' (coinciden)
        [PgName("Agencia")] 
        Agencia,
        
        // El nombre en C# es Guia, pero en BD es 'Guía' (con tilde)
        [PgName("Guía")] 
        Guia
    }
    public enum TipoResena
    {
        Destino,
        Agencia,
        Guia
    }
    public enum TipoFavorito
    {
        Destino,
        Lugar
    }
    public enum TipoHotelRest
    {
        Hotel,
        Restaurante
    }
}