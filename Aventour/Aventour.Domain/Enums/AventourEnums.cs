using NpgsqlTypes; // Necesitas este using (instala el paquete Npgsql si no te lo reconoce)

namespace Aventour.Domain.Enums
{
    public enum TipoAgenciaGuia
    {

        Agencia,
        

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