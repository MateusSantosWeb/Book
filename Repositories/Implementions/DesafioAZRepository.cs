using BookShelfAPI.Data;
using BookShelfAPI.Models;
using BookShelfAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookShelfAPI.Repositories.Implementions;

public class DesafioAZRepository  : Repository<DesafioAZ>, IDesafioAZRepository
{
    public DesafioAZRepository(BookShelfContext context) : base(context)
    {
    }

    public async Task<DesafioAZ?> GetByUsuarioAndAnoAsync(int usuario, int ano)
    {
        return await _dbSet
            .Include(d => d.Letras)
            .FirstOrDefaultAsync(d => d.UsuarioId == usuario && d.Ano == ano);
    }

    public async Task<DesafioAZ?> GetWithLetrasAsync(int id)
    {
        return await _dbSet
            .Include(d => d.Letras)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<LetraDesafio?> GetLetraAsync(int desafioId, string letra)
    {
        return await _context.LetrasDesafio
            .FirstOrDefaultAsync(l => l.DesafioAZId == desafioId && l.Letra == letra);
    }
}