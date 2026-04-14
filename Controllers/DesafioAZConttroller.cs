using Microsoft.AspNetCore.Mvc;
using BookShelfAPI.DTOs;
using BookShelfAPI.Services.Interfaces;

namespace BookShelfAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class DesafioAZController : ControllerBase
{
    private readonly IDesafioAZService _desafioService;

    public DesafioAZController(IDesafioAZService desafioService)
    {
        _desafioService = desafioService;
    }

    [HttpGet("usuario/{usuarioId}", Name = "GetDesafioPorUsuario")]
    public async Task<ActionResult<DesafioAZDto?>> GetDesafioPorUsuario(int usuarioId, [FromQuery] int? ano)
    {
        var desafio = await _desafioService.GetDesafioPorUsuarioAsync(usuarioId, ano);

        if (desafio == null)
            return NotFound(new { message = "Desafio A-Z não encontrado para este ano" });

        return Ok(desafio);
    }

    [HttpPost]
    public async Task<ActionResult<DesafioAZDto>> CreateDesafio(DesafioAZCreateDto dto)
    {
        try
        {
            var desafio = await _desafioService.CreateDesafioAsync(dto);
            return CreatedAtRoute("GetDesafioPorUsuario",
                new { usuarioId = desafio.UsuarioId, ano = desafio.Ano }, desafio);

        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{Id}/letra")]
    public async Task<IActionResult> AtualizarLetra(int Id, AtualizarLetraDto dto)
    {
        var updated = await _desafioService.AtualizarLetraAsync(Id, dto);
        
        if(!updated)
            return NotFound(new{message = "Desafio ou letra não encontrado "});
        
        return Ok(new{mesage = " Letra atualizada com sucesso "});
            
    }

    [HttpDelete("{Id}/letra/{letra}")]
    public async Task<IActionResult> LimaparLetra(int Id, string letra)
    {
        if (letra.Length != 1 || char.ToUpperInvariant(letra[0]) < 'A' || char.ToUpperInvariant(letra[0]) > 'Z')
            return BadRequest(new { message = " Letra Inválida. Use A-Z" });

        var cleared = await _desafioService.LimparLetraAsync(Id, letra);

        if (!cleared)
            return NotFound(new { message = "Desafio ou letra não encontrada" });

        return Ok(new { message = "Letra Limpa com sucesso" });

    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteDesafio(int Id)
    {
        var deleted = await _desafioService.DeleteDesafioAsync(Id);
        if (!deleted)
            return NotFound(new { message = "Desafio não encontrado " });
        
        return NoContent();
    }
}
