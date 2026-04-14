using BookShelfAPI.DTOs;
using BookShelfAPI.Models;

namespace BookShelfAPI.Services.Interfaces;

public interface IDesafioAZService
{
    Task<DesafioAZDto?> GetDesafioPorUsuarioAsync(int usuarioId, int? ano);
    Task<DesafioAZDto> CreateDesafioAsync(DesafioAZCreateDto dto);
    Task<bool> AtualizarLetraAsync(int desafioId, AtualizarLetraDto dto);
    Task<bool> LimparLetraAsync(int DesafioId, string letra);
    Task<bool> DeleteDesafioAsync(int id);
}   