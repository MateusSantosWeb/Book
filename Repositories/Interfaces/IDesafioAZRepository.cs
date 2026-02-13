using BookShelfAPI.Models;

namespace BookShelfAPI.Repositories.Interfaces;

public interface  DesafioAZRepository
{
    Task<DesafioAZ?> GetByUsuarioAndAnoAsync(int usuario, int  ano);
    Task<DesafioAZ?> GetWithLetrasAsync(int id);
    Task<LetraDesafio?> GetLetraAsync(int desafioId, string letra);
}