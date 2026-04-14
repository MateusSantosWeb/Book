using BookShelfAPI.DTOs;

namespace BookShelfAPI.Services.Interfaces;

public interface ILivroService
{
    Task<IEnumerable<LivroDto>> GetLivrosAsync(int? usuarioId, string? genero, bool? favorito);
    Task<LivroDto?> GetLivroByIdAsync(int id);
    Task<LivroDto> CreateLivroAsync(LivroCreateDto dto);
    Task<bool> UpdateLivroAsync(int id, LivroUpdateDto dto);
    Task<bool> DeleteLivroAsync(int id);
    Task<IEnumerable<string>> GetGenerosAsync(int? usuarioId);
}