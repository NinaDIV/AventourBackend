using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Persistence.Context;
using Aventour.Infrastructure.Persistence.Repositories;
using Aventour.Infrastructure.Repositories; // Asumo que Agencias y otros repositorios están aquí
using System.Threading.Tasks;
using System;

namespace Aventour.Infrastructure.Persistence.UnitOfWork
{
    // Clase que implementa el patrón Unit of Work y encapsula los repositorios
    public class UnitOfWork : IUnitOfWork, IDisposable
    
    {
        private readonly AventourDbContext _context;
        
        // --- Campos Privados para Lazy Loading ---
        private IFavoritoRepository? _favoritos;
        private IResenaRepository? _resenas;   // <-- CAMPO PRIVADO PARA RESEÑAS
        private IAgenciaRepository? _agencias; // <-- CAMPO PRIVADO PARA AGENCIAS
        private IDestinoRepository? _destinos;
        
        
        // Constructor con inyección de dependencia
        public UnitOfWork(AventourDbContext context)
        {
            _context = context;
        }

        // --- Propiedades Públicas (Inicialización Lazy) ---

        // Repositorio de Reseñas (CORREGIDO: Inicialización perezosa)
        public IResenaRepository Resenas => _resenas ??= new ResenaRepository(_context); 

        // Repositorio de Agencias (CORREGIDO: Inicialización perezosa)
        public IAgenciaRepository Agencias => _agencias ??= new AgenciaRepository(_context);

        // Repositorio de Favoritos
        public IFavoritoRepository Favoritos => _favoritos ??= new FavoritoRepository(_context);

        // Repositorio de Destinos
        public IDestinoRepository Destinos => _destinos ??= new DestinoRepository(_context);
        
        // --- Métodos de Persistencia y Limpieza ---

        // Método para persistir todos los cambios
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
        
        // Método duplicado, se recomienda usar solo CommitAsync, pero se mantiene para evitar romper código existente
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Método Dispose para liberar el contexto de la base de datos
        public void Dispose()
        {
            _context.Dispose();
           
            GC.SuppressFinalize(this);
        }
    }
}