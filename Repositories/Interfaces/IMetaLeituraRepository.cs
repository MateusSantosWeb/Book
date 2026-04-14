using BookShelfAPI.Models;

namespace BookShelfAPI.Repositories.Interfaces;

public interface IMetaLeituraRepository : IRepository<MetaLeitura>
{
    Task<MetaLeitura?> GetByUsuarioAndAnoAsync(int usuarioId, int ano);
    Task<MetaLeitura?> GetWithLivrosAsync(int id);
    Task<MetaLeituraLivro?> GetMetaLivrosAsync(int metaId, int livroId);
    Task AddLivroToMetaAsync(MetaLeituraLivro metaLivro);
    Task RemoveLivroFromMetaAsync(MetaLeituraLivro metaLivro);
}
