using BookShelfAPI.DTOs;
using BookShelfAPI.Models;

namespace BookShelfAPI.Services.Interfaces;

public interface IUsuarioServices
{
    Task<IEnumerable<UsuarioDto>> GetAllUsuariosAsync();
    Task<UsuarioDto?> GetUsuarioByIdAsync(int id);
    Task<UsuarioDto> CreateUsuarioAsync(UsuarioCreateDto dto);
    Task<bool> UpdateUsuarioAsync(int id, UsuarioCreateDto dto);
    Task<bool> DeleteUsuarioAsync(int id);
}