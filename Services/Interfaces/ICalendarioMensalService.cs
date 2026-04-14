using BookShelfAPI.DTOs;

namespace BookShelfAPI.Services.Interfaces;

public interface ICalendarioMensalService 
{
  Task<CalendarioAnualDto> GetCalendarioAnual(int usuarioId, int ano);
  Task<CalendarioMensalDto> GetCalendarioByIdAsync(int id);
  Task<CalendarioMensalDto> CreateCalendarioAsync(CalendarioMensalCreateDto dto);
  Task<bool> UpdateCalendarioAsync(int id, CalendarioMensalUpdateDto dto);
  Task<bool> UpdateoUCriarCalendarioAsync(int usuarioId, int ano, int mes, CalendarioMensalUpdateDto dto);
  Task<bool> DeleteCalendarioAsync(int id);
}
