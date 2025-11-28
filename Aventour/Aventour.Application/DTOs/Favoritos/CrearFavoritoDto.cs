namespace Aventour.Application.DTOs.Favoritos
{
    public class CrearFavoritoDto
    {
        // No pedimos IdUsuario aquí, lo sacaremos del Token para seguridad
        public int IdDestino { get; set; }
    }
}