using BookShelfAPI.Data;
using BookShelfAPI.DTOs;
using BookShelfAPI.Models;
using BookShelfAPI.Repositories.Interfaces;
using BookShelfAPI.Services.Interfaces;

namespace BookShelfAPI.Services.Implementations;

public class DesafioAZService : IDesafioAZService
{
    private readonly IDesafioAZRepository _desafioRepository;
    private readonly IRepository<Usuario> _usuarioRepository;
    private readonly BookShelfContext _context;
    

    public DesafioAZService(
        IDesafioAZRepository desafioRepository,
        IRepository<Usuario> usuarioRepository,
        BookShelfContext context)
    {
        _desafioRepository = desafioRepository;
        _usuarioRepository = usuarioRepository;
        _context = context;
    }
    
    public async Task<DesafioAZDto?> GetDesafioPorUsuarioAsync(int usuarioId, int? ano)
    {
        var anoFiltro = ano ?? DateTime.Now.Year;
        var desafio = await _desafioRepository.GetByUsuarioAndAnoAsync(usuarioId, anoFiltro);
        
        if (desafio == null)
            return null;

        var letrasDto = desafio.Letras
            .OrderBy(l => l.Letra)
            .Select(l => new LetraDesafioDto
            {
                Id = l.Id,
                Letra = l.Letra,
                TituloLivro = l.TituloLivro,
                Completado = l.Completado
            })
            .ToList();
        return new DesafioAZDto
        {
            Id = desafio.Id,
            Ano = desafio.Ano,
            UsuarioId = desafio.UsuarioId,
            Letras = letrasDto,
            TotalCompletado = letrasDto.Count(l => l.Completado)
        };
    }

    public async Task<DesafioAZDto> CreateDesafioAsync(DesafioAZCreateDto dto)
    {
        var usuarioExists = await _usuarioRepository.ExistsAsync(dto.UsuarioId);
        if (!usuarioExists)
            throw new InvalidOperationException("Usuário não encontrado");

        var desafioExistente = await _desafioRepository.GetByUsuarioAndAnoAsync(dto.UsuarioId, dto.Ano);
        if (desafioExistente != null)
            throw new InvalidOperationException("Já existe desafio A-Z para este ano");

        var desafio = new DesafioAZ
        {
            Ano = dto.Ano,
            UsuarioId = dto.UsuarioId
        };

        await _context.DesafiosAZ.AddAsync(desafio);
        await _context.SaveChangesAsync();
        
        var letras = new List<LetraDesafio>();
        for (char letra = 'A'; letra <= 'Z'; letra++)
        {
            letras.Add(new LetraDesafio
            {
                Letra = letra.ToString(),
                DesafioAZId = desafio.Id,
                Completado = false
            });
        }
        
        _context.LetrasDesafio.AddRange(letras);
        await _context.SaveChangesAsync();
        
        var letrasDto = letras.Select(l => new LetraDesafioDto
        {
            Id = l.Id,
            Letra = l.Letra,
            TituloLivro = l.TituloLivro,
            Completado = l.Completado
        })
        .ToList();

        return new DesafioAZDto
        {
            Id = desafio.Id,
            Ano = desafio.Ano,
            UsuarioId = desafio.UsuarioId,
            Letras = letrasDto,
            TotalCompletado = 0
        };
    }

    public async Task<bool> AtualizarLetraAsync(int desafioId, AtualizarLetraDto dto)
    {
        var desafio = await _desafioRepository.GetWithLetrasAsync(desafioId);
        if (desafio == null)
            return false;
        
        var letra = desafio.Letras.FirstOrDefault(l => l.Letra == dto.Letra);
        if (letra == null)
            return false;
        
        letra.TituloLivro = dto.TituloLivro;
        letra.Completado = !string.IsNullOrWhiteSpace(dto.TituloLivro);
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> LimparLetraAsync(int DesafioId, string letra)
    {
        var letraObj = await _desafioRepository.GetLetraAsync(DesafioId, letra.ToUpperInvariant());
        if (letraObj == null)
            return false;

        letraObj.TituloLivro = null;
        letraObj.Completado = false;
        await _context.SaveChangesAsync();
        return true;
    }
    

    

    public async Task<bool> DeleteDesafioAsync(int id)
    {

        var desafio = await _desafioRepository.GetByIdAsync(id);
        if (desafio == null)
            return false;
        
        await _desafioRepository.DeleteAsync(desafio);
        await _desafioRepository.SaveChangesAsync();

        return true;

    }
}
