using BookShelfAPI.Data;
using BookShelfAPI.Models;
using BookShelfAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookShelfAPI.Repositories.Implementions;

public class MetaLeituraRepository : Repository<MetaLeitura>, IMetaLeituraRepository
{
    public MetaLeituraRepository(BookShelfContext context) : base(context)
    {
    }

    public async Task<MetaLeitura?> GetByUsuarioAndAnoAsync(int usuarioId, int ano)
    {
        return await _dbSet
            .FirstOrDefaultAsync(m => m.UsuarioId == usuarioId && m.Ano == ano);
            
    }


    public async Task<MetaLeitura?> GetWithLivrosAsync(int id)
    {
        return await _dbSet
            .Include(m => m.LivrosNaMeta)
            .ThenInclude(ml => ml.Livro)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<MetaLeituraLivro?> GetMetaLivrosAsync(int metaId, int livroId)
    {
        return await _context.MetaLeituraLivros
            .FirstOrDefaultAsync(ml => ml.MetaLeituraId == metaId && ml.LivroId == livroId);
    }

    public async Task AddLivroToMetaAsync(MetaLeituraLivro metaLivro)
    {
        await _context.MetaLeituraLivros.AddAsync(metaLivro);
    }

    public async Task RemoveLivroFromMetaAsync(MetaLeituraLivro metaLivro)
    {
        _context.MetaLeituraLivros.Remove(metaLivro);
        await Task.CompletedTask;
    }
}
