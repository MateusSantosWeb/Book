using Microsoft.AspNetCore.Mvc;
using BookShelfAPI.DTOs;
using BookShelfAPI.Models;
using BookShelfAPI.Services.Interfaces;

namespace BookShelfAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class LivrosController : ControllerBase
{
    private readonly ILivroService _livroService;

    public LivrosController(ILivroService livroService)
    {
        _livroService = livroService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LivroDto>>> GetLivros(
        [FromQuery] int? usuarioId,
        [FromQuery] string? genero,
        [FromQuery] bool? favorito)
    {
        var livros = await _livroService.GetLivrosAsync(usuarioId, genero, favorito);
        return Ok(livros);
    }

    [HttpGet("{id}", Name = "GetLivroById")]
    public async Task<ActionResult<LivroDto>> GetLivro(int id)
    {
        var livro = await _livroService.GetLivroByIdAsync(id);

        if (livro == null) return NotFound(new { message = "Livro não encontrado" });
        return Ok(livro);
    }
    
    [HttpPost]
    public async Task<ActionResult<LivroDto>> CreateLivro( LivroCreateDto dto)
    {
        try
        {
            var livro = await _livroService.CreateLivroAsync(dto);
            return CreatedAtRoute("GetLivroById", new { id = livro.Id }, livro);

        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLivro(int id, LivroUpdateDto dto)
    {
        var updated = await _livroService.UpdateLivroAsync(id, dto);

        if (!updated)
            return NotFound(new { message = "Livro não encontrado" });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLivro(int id)
    {
        var deleted = await _livroService.DeleteLivroAsync(id);

        if (!deleted)
            return NotFound(new { message = "Livro não encontrado" });
        
        return NoContent();
        
    }

    [HttpGet("Generos")]
    public async Task<ActionResult<IEnumerable<string>>> GetLivros([FromQuery] int? usuarioId)
    {
        var generos = await _livroService.GetGenerosAsync(usuarioId);
        return Ok(generos);
    }
}
