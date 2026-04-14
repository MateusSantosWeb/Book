using BookShelfAPI.Models;

namespace BookShelfAPI.Repositories.Interfaces;

public interface ICalendarioMensalRepository : IRepository<CalendarioMensal>
{
    Task<IEnumerable<CalendarioMensal>> GetByUsuarioAndAnoAsync(int usuarioId, int ano);
    Task<CalendarioMensal> GetByUsuarioAnoMesAsync(int usuarioId, int ano, int mes);
}