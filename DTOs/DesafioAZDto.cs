using System.ComponentModel.DataAnnotations;
using BookShelfAPI.Models;

namespace BookShelfAPI.DTOs;

public class DesafioAZCreateDto
{
    [Required]
    [Range(2020,2100)]
    public int Ano { get; set; }
    
    [Required]
    public int UsuarioId { get; set; }
    
}

public class DesafioAZDto
{
    public int Id { get; set; }
    public int Ano { get; set; }
    public int UsuarioId { get; set; }
    public List<LetraDesafioDto> Letras { get; set; } = new();
    public int TotalCompletado { get; set; }
    public double Progresso => Math.Round((double)TotalCompletado / 26 * 100, 2);

}
public class LetraDesafioDto
{
    public int Id { get; set; }
    [Required]
    [MaxLength(1)]
    [RegularExpression("^[A-Z]$", ErrorMessage = "Dever ser uma Letra de A a Z")]
    public string Letra { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? TituloLivro { get; set; }

    public bool Completado { get; set; }
}

public class AtualizarLetraDto
{
    [Required]
    [MaxLength(1)]
    [RegularExpression("^[A-Z]$", ErrorMessage = "Dever ser uma Letra de A a Z")]
    public string Letra { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? TituloLivro { get; set; }

    public bool Completado { get; set; }
}
