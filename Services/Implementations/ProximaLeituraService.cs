using BookShelfAPI.DTOs;
using BookShelfAPI.Models;
using BookShelfAPI.Repositories.Interfaces;
using BookShelfAPI.Services.Interfaces;

namespace BookShelfAPI.Services.Implementations;

public class ProximaLeituraService : IProximaLeituraService
{
    private readonly IProximaLeitura _repository;

    public ProximaLeituraService(IProximaLeitura repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProximaLeituraDto>> GetAllAsync()
    {
        return (await _repository.GetAllAsync())
            .OrderBy(x => x.Prioridade)
            .Select(x => new ProximaLeituraDto
            {
                Id = x.Id,
                Titulo = x.Titulo,
                Autor = x.Autor,
                ImageUrl = x.ImageUrl,
                Complemento = x.Complemento,
                Prioridade = x.Prioridade
            });
    }

    public async Task<IEnumerable<ProximaLeituraDto>> AddAsync(ProximaLeituraDto dto)
    {
        var leitura = new ProximoLeitura
        {
            Titulo = dto.Titulo,
            Autor = string.IsNullOrWhiteSpace(dto.Autor) ? "Nao informado" : dto.Autor.Trim(),
            ImageUrl = dto.ImageUrl,
            Complemento = dto.Complemento,
            Prioridade = dto.Prioridade,
        };

        await _repository.AddAsync(leitura);
        await _repository.SaveChangesAsync();

        var listaOrdenada = (await _repository.GetAllAsync())
            .OrderBy(x => x.Prioridade)
            .Select(x => new ProximaLeituraDto
        {
            Id = x.Id,
            Titulo = x.Titulo,
            Autor = x.Autor,
            ImageUrl = x.ImageUrl,
            Complemento = x.Complemento,
            Prioridade = x.Prioridade
        });

        return listaOrdenada;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existente = await _repository.GetByIdAsync(id);
        if (existente is null)
        {
            return false;
        }

        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
        return true;
    }


}
