using BookShelfAPI.Data;
using BookShelfAPI.Models;
using BookShelfAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookShelfAPI.Repositories.Implementions;

public class CalendarioMensalRepository : Repository<CalendarioMensal> , ICalendarioMensalRepository
{
    public CalendarioMensalRepository(BookShelfContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CalendarioMensal>> GetByUsuarioAndAnoAsync(int usuarioId, int ano)
    {
        return await _dbSet
            .Where(x => x.UsuarioId == usuarioId && x.Ano == ano)
            .OrderBy(c => c.Id)
            .ToListAsync();
    }

    public async Task<CalendarioMensal> GetByUsuarioAnoMesAsync(int usuarioId, int ano, int mes)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Ano == ano && c.Mes == mes);
        
    }
}