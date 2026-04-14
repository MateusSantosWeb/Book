using System.Globalization;
using BookShelfAPI.DTOs;
using BookShelfAPI.Models;
using BookShelfAPI.Repositories.Interfaces;
using BookShelfAPI.Services.Interfaces;

namespace BookShelfAPI.Services.Implementations;

public class CalendarioMensalService : ICalendarioMensalService
{
    private readonly ICalendarioMensalRepository _calendarioRepository;
    
    private readonly IRepository<Usuario> _usuarioRepository;

    public CalendarioMensalService(ICalendarioMensalRepository calendarioRepository,
        IRepository<Usuario> usuarioRepository)
    {
        _calendarioRepository = calendarioRepository;
        _usuarioRepository = usuarioRepository;
    }
    
    
    public async Task<CalendarioAnualDto> GetCalendarioAnual(int usuarioId, int ano)
    {
        var calendarios = await _calendarioRepository.GetByUsuarioAndAnoAsync(usuarioId, ano);
        
        var mesesDto = calendarios.Select(c => new CalendarioMensalDto
        {
            Id = c.Id,
            Ano = c.Ano,
            Mes = c.Mes,
            NomeMes = ObterNomeMes(c.Mes),
            QuantidadeLivros = c.QuantidadeLivros,
            UsuarioId = c.UsuarioId
        }).ToList();

        for (int mes = 1; mes <= 12; mes++)
        {
            if (!mesesDto.Any(c => c.Mes == mes))
            {
                mesesDto.Add(new CalendarioMensalDto
                {
                    Id = mes,
                    Ano = ano,
                    Mes = mes,
                    NomeMes = ObterNomeMes(mes),
                    QuantidadeLivros = 0,
                    UsuarioId = usuarioId
                });

            }
        }


        mesesDto = mesesDto.OrderBy(m => m.Mes).ToList();

        var totalLivros = mesesDto.Sum(m => m.QuantidadeLivros);
        var mesMaisLido = mesesDto
            .OrderByDescending(m => m.QuantidadeLivros)
            .FirstOrDefault()?.NomeMes ?? "Nenhum";

        return new CalendarioAnualDto
        {
            Ano = ano,
            Meses = mesesDto,
            TotalLivrosAno = totalLivros,
            MesMaisLido = mesMaisLido
        };
    }

    public async Task<CalendarioMensalDto> GetCalendarioByIdAsync(int id)
    {
        var calendario = await _calendarioRepository.GetByIdAsync(id);

        if (calendario == null)
            return null;
        
        return new CalendarioMensalDto
            {
                Id = calendario.Id,
                Ano = calendario.Ano,
                Mes = calendario.Mes,
                NomeMes = ObterNomeMes(calendario.Mes),
                QuantidadeLivros = calendario.QuantidadeLivros,
                UsuarioId = calendario.UsuarioId
            };
        
    }

    public async Task<CalendarioMensalDto> CreateCalendarioAsync(CalendarioMensalCreateDto dto)
    {
        var usuarioExists = await _usuarioRepository.ExistsAsync(dto.UsuarioId);
        if (!usuarioExists)
            throw new InvalidOperationException("Usuário não encontrado");

        var calendarioExistente = await _calendarioRepository
            .GetByUsuarioAnoMesAsync(dto.UsuarioId, dto.Ano, dto.Mes);

        if (calendarioExistente != null)
            throw new InvalidOperationException("Já existe calendário para este mês");

        var calendario = new CalendarioMensal
        {
            Ano = dto.Ano,
            Mes = dto.Mes,
            QuantidadeLivros = dto.QuantidadeLivros,
            UsuarioId = dto.UsuarioId
        };

        await _calendarioRepository.AddAsync(calendario);
        await _calendarioRepository.SaveChangesAsync();

        return new CalendarioMensalDto
        {
            Id = calendario.Id,
            Ano = calendario.Ano,
            Mes = calendario.Mes,
            NomeMes = ObterNomeMes(calendario.Mes),
            QuantidadeLivros = calendario.QuantidadeLivros,
            UsuarioId = calendario.UsuarioId
        };
    }

    public async Task<bool> UpdateCalendarioAsync(int id, CalendarioMensalUpdateDto dto)
    {
        var calendario = await _calendarioRepository.GetByIdAsync(id);
        if (calendario == null)
            return false;

        calendario.QuantidadeLivros = dto.QuantidadeLivros;
        await _calendarioRepository.UpdateAsync(calendario);
        await _calendarioRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateoUCriarCalendarioAsync(int usuarioId, int ano, int mes, CalendarioMensalUpdateDto dto)
    {
        if (mes < 1 || mes > 12)
            throw new InvalidOperationException("Mes invalido");
        
        var calendario = await _calendarioRepository.GetByUsuarioAnoMesAsync(usuarioId, ano, mes);

        if (calendario == null)
        {
            calendario = new CalendarioMensal
            {
                UsuarioId = usuarioId,
                Ano = ano,
                Mes = mes,
                QuantidadeLivros = dto.QuantidadeLivros
            };
            await _calendarioRepository.AddAsync(calendario);
            
        }
        else
        {
            calendario.QuantidadeLivros = dto.QuantidadeLivros;
            await _calendarioRepository.UpdateAsync(calendario);
        }

        await _calendarioRepository.SaveChangesAsync();
        return true;
        
    }

    public async Task<bool> DeleteCalendarioAsync(int id)
    {
        var calendario = await _calendarioRepository.GetByIdAsync(id);
        if (calendario == null)
            return false;
        
        await _calendarioRepository.DeleteAsync(calendario);
        await _calendarioRepository.SaveChangesAsync();

        return true;
    }

    private static string ObterNomeMes(int mes)
    {
        var cultura = new CultureInfo("pt-BR");
        return cultura.DateTimeFormat.GetMonthName(mes);
    }
}
