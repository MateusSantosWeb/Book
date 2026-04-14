using BookShelfAPI.DTOs;
using BookShelfAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookShelfAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
[Route("[controller]")]
[Route("api/proxima-meta")]
[Route("proxima-meta")]

public class ProximaLeituraController : ControllerBase
{
    private readonly IProximaLeituraService _proximaLeitura;

    public ProximaLeituraController(IProximaLeituraService proximaLeitura)
    {
        _proximaLeitura = proximaLeitura;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var livros = await _proximaLeitura.GetAllAsync();
        return Ok(livros);
    }

    [HttpPost]
    public async Task<IActionResult> Post(ProximaLeituraDto dto)
    {
        await _proximaLeitura.AddAsync(dto);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var removed = await _proximaLeitura.DeleteAsync(id);
        if (!removed)
        {
            return NotFound(new { message = "Livro não encontrado" });
        }

        return NoContent();
    }
}
