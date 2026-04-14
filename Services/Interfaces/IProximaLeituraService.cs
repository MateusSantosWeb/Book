using BookShelfAPI.DTOs;

namespace BookShelfAPI.Services.Interfaces;

public interface IProximaLeituraService
{
    Task<IEnumerable<ProximaLeituraDto>> GetAllAsync();
    Task<IEnumerable<ProximaLeituraDto>> AddAsync(ProximaLeituraDto dto);
    Task<bool> DeleteAsync(int id);
}
