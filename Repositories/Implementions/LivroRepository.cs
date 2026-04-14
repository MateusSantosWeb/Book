using BookShelfAPI.Data;
using BookShelfAPI.Models;
using BookShelfAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookShelfAPI.Repositories.Implementions;

public class LivroRepository : Repository<Livro>, ILivroRepository
{
    
    
    public LivroRepository(BookShelfContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Livro>> GetByUsuarioIdAsync(int usuarioId)
    {
        return await _dbSet
            .Where(l => l.UsuarioId == usuarioId)
            .OrderByDescending(l => l.DataConclusao)
            .ToListAsync();
    }

    public async Task<IEnumerable<Livro>> GetByGeneroAsync(string genero, int? usuarioId = null)
    {
        var query = _dbSet.Where(l => l.Genero == genero);
        if (usuarioId.HasValue)
            query = query.Where(l => l.UsuarioId == usuarioId.Value);

        return await query.OrderByDescending(l => l.DataConclusao).ToListAsync();
        
    }

    public async Task<IEnumerable<Livro>> GetFavoritosAsync(int usuarioId)
    {
        return await _dbSet.Where(l => l.UsuarioId == usuarioId)
            .OrderByDescending(l => l.DataConclusao)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetGeneroAsync(int? usuarioId = null)
    {
        var query = _dbSet.Where(l => l.UsuarioId == usuarioId.Value);
        
        return await query
            .Select(l => l.Genero!)
            .Distinct()
            .OrderBy(g => g)
            .ToListAsync();
    }
}