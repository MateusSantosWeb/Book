using BookShelfAPI.DTOs;
using BookShelfAPI.Models;
using BookShelfAPI.Repositories.Interfaces;
using BookShelfAPI.Services.Interfaces;

namespace BookShelfAPI.Services.Implementations;

public class LivroService : ILivroService
{
    private readonly ILivroRepository _livroRepository;
    private readonly IRepository<Usuario> _usuarioRepository;
    private readonly IMetaLeituraRepository _metaRepository;
    private readonly IProximaLeitura _proximaLeituraRepository;

    public LivroService(
        ILivroRepository livroRepository,
        IRepository<Usuario> usuarioRepository,
        IMetaLeituraRepository metaRepository,
        IProximaLeitura proximaLeituraRepository)
    {
        _livroRepository = livroRepository;
        _usuarioRepository = usuarioRepository;
        _metaRepository = metaRepository;
        _proximaLeituraRepository = proximaLeituraRepository;
    }
        
    public async Task<IEnumerable<LivroDto>> GetLivrosAsync(int? usuarioId, string? genero, bool? favorito)
    {
        IEnumerable<Livro> livros;

        if (usuarioId.HasValue && !string.IsNullOrEmpty(genero))
        {
            livros = await _livroRepository.GetByGeneroAsync(genero, usuarioId.Value);
        }
        else if (usuarioId.HasValue && favorito == true)
        {
            livros = await _livroRepository.GetFavoritosAsync(usuarioId.Value);
        }
        else if (usuarioId.HasValue)
        {
            livros = await _livroRepository.GetByUsuarioIdAsync(usuarioId.Value);
        }
        else
        {
            livros = await _livroRepository.GetAllAsync();
        }

        if (favorito.HasValue && !usuarioId.HasValue)
        {
            livros = livros.Where(livro => livro.Favoarito == favorito.Value);
        }

        return livros.Select(MapToDto);
    }

    public async Task<LivroDto?> GetLivroByIdAsync(int id)
    {
        var livro = await _livroRepository.GetByIdAsync(id);
        return MapToDto(livro);
    }

    public async Task<LivroDto> CreateLivroAsync(LivroCreateDto dto)
    {
        var usuarioExists = await _usuarioRepository.ExistsAsync(dto.UsuarioId);
        if (!usuarioExists)
            throw new InvalidOperationException("Usuário não encontrado");
        var livro = new Livro
        {
            Titulo = dto.Titulo,
            Autor = dto.Autor,
            ImageUrl = dto.ImagemUrl,
            Genero = dto.Genero,
            TempoDeLeiturasDias = dto.TempoLeituraDias,
            Estrelas = dto.Estrelas,
            Coracoes = dto.Coracoes,
            Fogos = dto.Fogos,
            Humor = dto.Humor,
            Favoarito = dto.Favorito,
            DataConclusao = dto.DataConclusao.HasValue
                ? NormalizeToUtc(dto.DataConclusao.Value)
                : DateTime.UtcNow,
            UsuarioId = dto.UsuarioId
        };
        
        await _livroRepository.AddAsync(livro);
        await _livroRepository.SaveChangesAsync();

        var anoMeta = livro.DataConclusao.Year;
        var meta = await _metaRepository.GetByUsuarioAndAnoAsync(livro.UsuarioId, anoMeta);
        if (meta != null)
        {
            var relacaoExistente = await _metaRepository.GetMetaLivrosAsync(meta.Id, livro.Id);
            if (relacaoExistente == null)
            {
                await _metaRepository.AddLivroToMetaAsync(new MetaLeituraLivro
                {
                    MetaLeituraId = meta.Id,
                    LivroId = livro.Id
                });

                meta.QuantidadeLida += 1;
                await _metaRepository.UpdateAsync(meta);
                await _metaRepository.SaveChangesAsync();
            }
        }

        var proximasLeituras = await _proximaLeituraRepository.GetAllAsync();
        var jaExisteNaProximaMeta = proximasLeituras.Any(x =>
            string.Equals(x.Titulo.Trim(), livro.Titulo.Trim(), StringComparison.OrdinalIgnoreCase) &&
            string.Equals(x.Autor.Trim(), livro.Autor.Trim(), StringComparison.OrdinalIgnoreCase));

        if (!jaExisteNaProximaMeta)
        {
            await _proximaLeituraRepository.AddAsync(new ProximoLeitura
            {
                Titulo = livro.Titulo,
                Autor = string.IsNullOrWhiteSpace(livro.Autor) ? "Nao informado" : livro.Autor.Trim(),
                ImageUrl = livro.ImageUrl,
                Prioridade = 0
            });
            await _proximaLeituraRepository.SaveChangesAsync();
        }

        return MapToDto(livro);
    }

    public async Task<bool> UpdateLivroAsync(int id, LivroUpdateDto dto)
    {
        var livro = await _livroRepository.GetByIdAsync(id);
        if(livro == null)
            return false;
        
        if(dto.Titulo != null) livro.Titulo = dto.Titulo;
        if(dto.Autor != null) livro.Autor = dto.Autor;
        if(dto.ImagemUrl != null) livro.ImageUrl = dto.ImagemUrl;
        if(dto.Genero != null) livro.Genero = dto.Genero;
        if(dto.TempoLeituraDias.HasValue) livro.TempoDeLeiturasDias = dto.TempoLeituraDias.Value;
        if(dto.Estrelas.HasValue) livro.Estrelas = dto.Estrelas.Value;
        if(dto.Coracoes != null) livro.Coracoes = dto.Coracoes.Value;
        if(dto.Fogos != null) livro.Fogos = dto.Fogos.Value;
        if(dto.Humor != null) livro.Humor = dto.Humor.Value;
        if(dto.Favorito.HasValue) livro.Favoarito = dto.Favorito.Value;
        if (dto.DataConclusao.HasValue) livro.DataConclusao = NormalizeToUtc(dto.DataConclusao.Value);
        
        await _livroRepository.UpdateAsync(livro);
        await _livroRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteLivroAsync(int id)
    {
        var livro = await _livroRepository.GetByIdAsync(id);
        if (livro == null)
            return false;
        
        await _livroRepository.DeleteAsync(livro);
        await _livroRepository.SaveChangesAsync();
        
        return true;
    }

    public async Task<IEnumerable<string>> GetGenerosAsync(int? usuarioId)
    {
        return await _livroRepository.GetGeneroAsync(usuarioId);
        
    }

    private LivroDto MapToDto(Livro livro)
    {
        return new LivroDto
        {
            Id = livro.Id,
            Titulo = livro.Titulo,
            Autor = livro.Autor,
            ImagemUrl = livro.ImageUrl,
            Genero = livro.Genero,
            TempoLeituraDias = livro.TempoDeLeiturasDias,
            Estrelas = livro.Estrelas,
            Coracoes = livro.Coracoes,
            Fogos = livro.Fogos,
            Humor = livro.Humor,
            Favorito = livro.Favoarito,
            DataConclusao = livro.DataConclusao,
            UsuarioId = livro.UsuarioId
        };
    }

    private static DateTime NormalizeToUtc(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
    }
}
