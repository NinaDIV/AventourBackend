using Aventour.Domain.Entities;
using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Persistence.Context;

namespace Aventour.Infrastructure.Persistence.Repositories;

public class AgenciaRepository : IAgenciaRepository
{
    private readonly AventourDbContext _context;

    public AgenciaRepository(AventourDbContext context)
    {
        _context = context;
    }

    public async Task<AgenciasGuia> GetByIdAsync(int id)
    {
        return await _context.AgenciasGuias.FindAsync(id);
    }

    // Otros métodos (Add, Delete, etc)
}
