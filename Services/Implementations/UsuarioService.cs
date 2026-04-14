using BookShelfAPI.DTOs;
using BookShelfAPI.Models;
using BookShelfAPI.Repositories.Interfaces;
using BookShelfAPI.Services.Interfaces;

namespace BookShelfAPI.Services.Implementations;

public class UsuarioService : IUsuarioServices
{
    private readonly IRepository<Usuario>  _usuarioRepository;
    
    public UsuarioService(IRepository<Usuario> usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }
    
    public async Task<IEnumerable<UsuarioDto>> GetAllUsuariosAsync()
    {
        var usuarios = await _usuarioRepository.GetAllAsync();
        return usuarios.Select(MapToDto);
    }
    

    public async Task<UsuarioDto?> GetUsuarioByIdAsync(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        return usuario != null ? MapToDto(usuario) : null;
    }
    

    public async Task<UsuarioDto> CreateUsuarioAsync(UsuarioCreateDto dto)
    {
        var usuario = new Usuario
        {
            Nome = dto.Nome,
            DataCriacao = DateTime.UtcNow
        };
        await _usuarioRepository.AddAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();
        
        return MapToDto(usuario);
    }

    public async Task<bool> UpdateUsuarioAsync(int id, UsuarioCreateDto dto)
    {
        var usuario =  await _usuarioRepository.GetByIdAsync(id);
        if(usuario == null)
            return false;
        
        usuario.Nome = dto.Nome;
        
        await _usuarioRepository.UpdateAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();
        return true;
    }
    

    public async Task<bool> DeleteUsuarioAsync(int id)
    {
        var usuario =  await _usuarioRepository.GetByIdAsync(id);
        if(usuario == null)
            return false;
        
        await _usuarioRepository.DeleteAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();
        return true;
    }
    private UsuarioDto MapToDto(Usuario usuario)
    {
        return new UsuarioDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            DataCriacao = usuario.DataCriacao
        };
    }
}