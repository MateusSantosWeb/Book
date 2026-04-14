using Microsoft.AspNetCore.Mvc;
using BookShelfAPI.DTOs;
using BookShelfAPI.Services.Interfaces;

namespace BookShelfAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class MetaLeituraController : ControllerBase
{
    private readonly IMetaLeituraService _metaService;

    public MetaLeituraController(IMetaLeituraService metaService)
    {
        _metaService = metaService;
    }

    [HttpGet("usuario/{usuarioId}", Name = "GetMetaPorUsuario")]
    public async Task<ActionResult<MetaLeituraComLivrosDto>> GetMetaPorUsuario(int usuarioId, [FromQuery] int? ano)
    {
        var meta = await _metaService.GetMetaPorUsuarioAsync(usuarioId, ano);

        if (meta == null)
            return NotFound(new { message = "Meta de leitura não encontrada para este ano" });
        
        return Ok(meta);
    }

    [HttpPost]
    public async Task<ActionResult<MetaDeLeituraDto>> CreateMeta(MetaDeLeituraCreayeDto dto)
    {

        try
        {
            var meta = await _metaService.CreateMetaAsync(dto);
            return CreatedAtRoute("GetMetaPorUsuario",
                new { usuarioId = meta.UsuarioId, ano = meta.Ano }, meta);
        }
        catch(InvalidOperationException ex)
        {
            return BadRequest(new{message = ex.Message});
        }
        
        
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMeta(int id, MetaDeLeituraUpdateDto dto)
    {
        var updated = await _metaService.UpdateMetaAsync(id, dto);

        if (!updated)
            return NotFound(new { message = "Meta não encontrada" });

        return NoContent();
    }

    [HttpPost("{id}/livros")]

    public async Task<IActionResult> AdicionarLivroNaMeta(int id, AdicionarLivroMetaDto dto)
    {
        try
        {
            await _metaService.AdicionarLivroNaMetaAsync(id, dto.LivroId);
            return Ok(new{message = "Livro Adicionado com sucesso"});
            
        }
        catch(InvalidCastException ex)
        {
            return BadRequest(new{message = ex.Message});
        }
    }

    [HttpDelete("{id}/livros/{livroId}")]
    public async Task<IActionResult> RemoverLivroNaMeta(int id, int livroId)
    {
        var removed = await _metaService.RemoverLivroDaMetaAsync(id, livroId);

        if (!removed)
            return NotFound(new { message = "Livro não encontrado" });
        
        return Ok(new{message = "Livro removido com sucesso"});
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeta(int id)
    {
        var deleted = await _metaService.DeleteMetaAsync(id);

        if (!deleted)
            return NotFound(new { message = "Meta não encontrada" });
        
        return NoContent();
    }
    
}
