
using Aventour.Application.DTOs.Usuarios;
using Aventour.Domain.Entities;
using Aventour.Domain.Enums;
using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Authentication; // Referencia al generador de token
using BCrypt.Net; // Asegúrate de instalar BCrypt.Net-Next

namespace Aventour.Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public UsuarioService(IUsuarioRepository usuarioRepository, JwtTokenGenerator jwtTokenGenerator)
        {
            _usuarioRepository = usuarioRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        // 1. REGISTRO
        public async Task<UsuarioResponseDto> RegistrarUsuarioAsync(CrearUsuarioDto dto)
        {
            // Validar si el email ya existe
            var existente = await _usuarioRepository.GetByEmailAsync(dto.Email);
            if (existente != null) throw new Exception("El correo ya está registrado.");

            // Hashear contraseña
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            
            // Mapeo de Enum (DTO) -> Boolean (Entidad DB)
            bool esAdmin = dto.Rol == RolUsuario.Admin;

            var nuevoUsuario = new Usuario
            {
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Edad = dto.Edad,
                FechaRegistro = DateTime.Now, // Kind=Local (PostgreSQL lo acepta en timestamp without time zone)
                // Aquí asignamos el valor booleano
                EsAdministrador = esAdmin,
                SesionActiva = false
            };

            await _usuarioRepository.AddAsync(nuevoUsuario);
            // Generar token (ahora incluirá el rol)
            var token = _jwtTokenGenerator.GenerateToken(nuevoUsuario);

            return new UsuarioResponseDto
            {
                IdUsuario = nuevoUsuario.IdUsuario,
                Nombres = nuevoUsuario.Nombres,
                Apellidos = nuevoUsuario.Apellidos,
                Email = nuevoUsuario.Email,
                Token = token,
                Rol = dto.Rol.ToString() // Devolvemos "Admin" o "Turista"
            };
        }

        // 2. LOGIN (Autenticación JWT)
        public async Task<UsuarioResponseDto> LoginAsync(LoginDto dto)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(dto.Email);
            
            // Validar usuario y contraseña
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
            {
                throw new Exception("Credenciales inválidas.");
            }

            // Generar Token
            var token = _jwtTokenGenerator.GenerateToken(usuario);

            // Actualizar sesión activa (opcional según tu lógica)
            usuario.SesionActiva = true;
            await _usuarioRepository.UpdateAsync(usuario);

            return new UsuarioResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Email = usuario.Email,
                EsAdministrador = usuario.EsAdministrador ?? false,
                Token = token // Aquí devolvemos el JWT
            };
        }

        // 3. GET BY ID
        public async Task<UsuarioResponseDto> ObtenerPorIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null) return null;

            return new UsuarioResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Email = usuario.Email,
                EsAdministrador = usuario.EsAdministrador ?? false
            };
        }

        // 4. UPDATE
        public async Task ActualizarUsuarioAsync(int id, UpdateUsuarioDto dto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null) throw new Exception("Usuario no encontrado.");

            usuario.Nombres = dto.Nombres ?? usuario.Nombres;
            usuario.Apellidos = dto.Apellidos ?? usuario.Apellidos;
            usuario.Edad = dto.Edad ?? usuario.Edad;
            usuario.EstadoCivil = dto.EstadoCivil ?? usuario.EstadoCivil;

            await _usuarioRepository.UpdateAsync(usuario);
        }
    }
}