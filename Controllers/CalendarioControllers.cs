using Microsoft.AspNetCore.Mvc;
using global::BookShelfAPI.DTOs;
using BookShelfAPI.Services.Interfaces;

namespace BookShelfAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalendarioController : ControllerBase
{
    private readonly ICalendarioMensalService _calendarioService;

    public CalendarioController(ICalendarioMensalService calendarioService)
    {
        _calendarioService = calendarioService;
    }

    [HttpGet("usuario/{usuarioId}/ano/{ano}")]
    public async Task<ActionResult<CalendarioAnualDto>> GetCalendarioAnual(int usuarioId, int ano)
    {
        var calendario = await _calendarioService.GetCalendarioAnual(usuarioId, ano);
        return Ok(calendario);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CalendarioMensalDto>> GetCalendario(int id)
    {
        var calendario = await _calendarioService.GetCalendarioByIdAsync(id);

        if (calendario == null)
            return NotFound(new { message = "Calendário não encontrado" });

        return Ok(calendario);
    }

    [HttpPost]
    public async Task<ActionResult<CalendarioMensalDto>> CreateCalendario(CalendarioMensalCreateDto dto)
    {
        try
        {
            var calendario = await _calendarioService.CreateCalendarioAsync(dto);
            return CreatedAtAction(nameof(GetCalendario), new { id = calendario.Id }, calendario);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCalendario(int id, CalendarioMensalUpdateDto dto)
    {
        var updated = await _calendarioService.UpdateCalendarioAsync(id, dto);

        if (!updated)
            return NotFound(new { message = "Calendário não encontrado" });

        return NoContent();
    }

    [HttpPut("usuario/{usuarioId}/ano/{ano}/mes/{mes}")]
    public async Task<IActionResult> UpdateOuCriarCalendario(int usuarioId, int ano, int mes, CalendarioMensalUpdateDto dto)
    {
        try
        {
            await _calendarioService.UpdateoUCriarCalendarioAsync(usuarioId, ano, mes, dto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCalendario(int id)
    {
        var deleted = await _calendarioService.DeleteCalendarioAsync(id);

        if (!deleted)
            return NotFound(new { message = "Calendário não encontrado" });

        return NoContent();
    }
}
