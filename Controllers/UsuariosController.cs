using Microsoft.AspNetCore.Mvc;
using BookShelfAPI.DTOs;
using BookShelfAPI.Services.Interfaces;

namespace BookShelfAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UsuariosController : ControllerBase
{
    private readonly IUsuarioServices _usuarioService;

    public UsuariosController(IUsuarioServices usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
    {
        var usuarios = await _usuarioService.GetAllUsuariosAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id}", Name = "GetUsuarioById")]
    public async Task<ActionResult<UsuarioDto>> GetUsuarioAsync(int id)
    {
        var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
        
        if(usuario == null)
            return NotFound(new{message = "Usuario não encontrado"});
        
        return Ok(usuario);
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioDto>> CreateUsuario(UsuarioCreateDto dto)
    {
        var usuario = await _usuarioService.CreateUsuarioAsync(dto);
        return CreatedAtRoute("GetUsuarioById", new { id = usuario.Id }, usuario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUsuarioAsync(int id, UsuarioCreateDto dto)
    {
        var updated = await _usuarioService.UpdateUsuarioAsync(id, dto);

        if (!updated)
            return NotFound(new { message = "Usuário não encontrado" });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuarioAsync(int id)
    {
        var deleted = await _usuarioService.DeleteUsuarioAsync(id);

        if (!deleted)
            return NotFound(new { message = "Usuário não encontrado" });

        return NoContent();
    }


}
    
    
