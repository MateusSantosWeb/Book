using BookShelfAPI.Models;

namespace BookShelfAPI.Repositories.Interfaces;

public interface ILivroRepository : IRepository<Livro>
{
    Task<IEnumerable<Livro>> GetByUsuarioIdAsync(int usuarioId);
    Task<IEnumerable<Livro>> GetByGeneroAsync(string genero, int? usuarioId = null);
    Task<IEnumerable<Livro>> GetFavoritosAsync(int usuarioId);
    Task<IEnumerable<string>> GetGeneroAsync (int? usuarioId = null);
}