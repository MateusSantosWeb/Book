using BookShelfAPI.DTOs;
using BookShelfAPI.Models;
using BookShelfAPI.Repositories.Interfaces;
using BookShelfAPI.Services.Interfaces;

namespace BookShelfAPI.Services.Implementations;

public class MetaLeituraService : IMetaLeituraService
{
    private readonly IMetaLeituraRepository _metaRepository;
    private readonly IRepository<Usuario> _usuarioRepository;
    private readonly ILivroRepository _livroRepository;

    public MetaLeituraService(
        IMetaLeituraRepository metaRepository,
        IRepository<Usuario> usuarioRepository,
        ILivroRepository livroRepository)
    {
        _metaRepository = metaRepository;
        _usuarioRepository = usuarioRepository;
        _livroRepository = livroRepository;
    }
    
    public async  Task<MetaLeituraComLivrosDto?> GetMetaPorUsuarioAsync(int usuarioId, int? ano)
    {
        var anoFiltro = ano ?? DateTime.Now.Year;
        var meta = await _metaRepository.GetByUsuarioAndAnoAsync(usuarioId, anoFiltro);
        
        if (meta == null)
            return null;

        meta = await _metaRepository.GetWithLivrosAsync(meta.Id);
        if(meta == null)
            return null;

        var livros = meta.LivrosNaMeta
            .Select(ml => new LivroDto
            {
                Id = ml.Livro.Id,
                Titulo = ml.Livro.Titulo,
                Autor = ml.Livro.Autor,
                ImagemUrl = ml.Livro.ImageUrl,
                Genero = ml.Livro.Genero,
                TempoLeituraDias = ml.Livro.TempoDeLeiturasDias,
                Estrelas = ml.Livro.Estrelas,
                Coracoes = ml.Livro.Coracoes,
                Fogos = ml.Livro.Fogos,
                Humor = ml.Livro.Humor,
                Favorito = ml.Livro.Favoarito,
                DataConclusao = ml.Livro.DataConclusao,
                UsuarioId = ml.Livro.UsuarioId

            })
            .OrderByDescending(l => l.DataConclusao)
            .ToList();
        
        var generosMaisLidos = livros.Where(l => !string.IsNullOrEmpty(l.Genero))
            .GroupBy(l => l.Genero!)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key)
            .Take(3)
            .ToDictionary(g => g.Key, g => g.Count());

        var top3Generos = generosMaisLidos
            .Select(g => new GeneroContagemDto
            {
                Nome = g.Key,
                Quantidade = g.Value
            })
            .ToList();

        while (top3Generos.Count < 3)
        {
            top3Generos.Add(new GeneroContagemDto
            {
                Nome = $"Genero {top3Generos.Count + 1}",
                Quantidade = 0
            });
        }

        return new MetaLeituraComLivrosDto
        {
            Id = meta.Id,
            Ano = meta.Ano,
            QuantidadeObejetivo = meta.QuantidadeObjetivo,
            QuantidadeLida = meta.QuantidadeLida,
            UsuarioId = meta.UsuarioId,
            Livros = livros,
            GenerosMaisLidos = generosMaisLidos,
            Top3Generos = top3Generos
        };
    }

    public async Task<MetaDeLeituraDto> CreateMetaAsync(MetaDeLeituraCreayeDto dto)
    {
        var usuarioExists = await _usuarioRepository.ExistsAsync(dto.UsuarioId);
        if (!usuarioExists)
            throw new InvalidOperationException("Usuário não encontrado");

        var metaJaExiste = await _metaRepository.GetByUsuarioAndAnoAsync(dto.UsuarioId, dto.Ano);
        if (metaJaExiste != null)
            throw new InvalidOperationException("Já existe uma meta de leitura para este ano");

        var meta = new MetaLeitura
        {
            Ano = dto.Ano,
            QuantidadeObjetivo = dto.QuantidadeObejetivo,
            QuantidadeLida = 0,
            UsuarioId = dto.UsuarioId
        };

        await _metaRepository.AddAsync(meta);
        await _metaRepository.SaveChangesAsync();

        return new MetaDeLeituraDto
        {
            Id = meta.Id,
            Ano = meta.Ano,
            QuantidadeObjetivo = meta.QuantidadeObjetivo,
            QuantidadeLida = meta.QuantidadeLida,
            UsuarioId = meta.UsuarioId
        };
    }

    public async Task<bool> UpdateMetaAsync(int id, MetaDeLeituraUpdateDto dto)
    {
        var meta = await _metaRepository.GetByIdAsync(id);
        if (meta == null)
            return false;

        if (dto.QuantidadeObejetivo.HasValue)
            meta.QuantidadeObjetivo = dto.QuantidadeObejetivo.Value;

        await _metaRepository.UpdateAsync(meta);
        await _metaRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteMetaAsync(int id)
    {
        var meta = await _metaRepository.GetByIdAsync(id);
        if (meta == null)
            return false;

        await _metaRepository.DeleteAsync(meta);
        await _metaRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AdicionarLivroNaMetaAsync(int metaId, int livroId)
    {
        var meta = await _metaRepository.GetByIdAsync(metaId);
        if (meta == null)
            return false;

        var livroExiste = await _livroRepository.ExistsAsync(livroId);
        if (!livroExiste)
            return false;

        var relacaoExistente = await _metaRepository.GetMetaLivrosAsync(metaId, livroId);
        if (relacaoExistente != null)
            return false;

        await _metaRepository.AddLivroToMetaAsync(new MetaLeituraLivro
        {
            MetaLeituraId = metaId,
            LivroId = livroId
        });

        meta.QuantidadeLida += 1;
        await _metaRepository.UpdateAsync(meta);
        await _metaRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoverLivroDaMetaAsync(int metaId, int livroId)
    {
        var meta = await _metaRepository.GetByIdAsync(metaId);
        if (meta == null)
            return false;

        var relacao = await _metaRepository.GetMetaLivrosAsync(metaId, livroId);
        if (relacao == null)
            return false;

        await _metaRepository.RemoveLivroFromMetaAsync(relacao);
        if (meta.QuantidadeLida > 0)
            meta.QuantidadeLida -= 1;

        await _metaRepository.UpdateAsync(meta);
        await _metaRepository.SaveChangesAsync();

        return true;
    }
}
