using System.Security.Cryptography;
 
namespace Aventour.Infrastructure.Security;

public class PasswordHasher
{
    public string Hash(string password)
    {
        // Implementación simple con BCrypt recomendada, 
        // aquí usaremos BCrypt.Net si instalas el paquete 'BCrypt.Net-Next'
        // O una implementación nativa. Para simplificar, usaremos BCrypt.
        // Asegúrate de agregar el paquete: dotnet add package BCrypt.Net-Next
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}