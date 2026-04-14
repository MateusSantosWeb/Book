using BookShelfAPI.Models;

namespace BookShelfAPI.Repositories.Interfaces;

public interface IProximaLeitura
{
    Task<IEnumerable<ProximoLeitura>> GetAllAsync();
    Task<ProximoLeitura?> GetByIdAsync(int id);
    Task AddAsync(ProximoLeitura proximoLeitura);
    Task UpdateAsync(ProximoLeitura proximoLeitura);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
