using System.ComponentModel.DataAnnotations;
using BookShelfAPI.Models;

namespace BookShelfAPI.DTOs;

public class LivroCreateDto
{
    [Required(ErrorMessage = "Título obrigatório")]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Autor é obrigatório")]
    [MaxLength(150)]
    public string Autor { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? ImagemUrl { get; set; }
    
    [MaxLength(50)]
    public string? Genero { get; set; }
    
    [Range(1, 365, ErrorMessage = "O tempo de Leitura deve ser entre 1 e 365 dias")]
    public int TempoLeituraDias { get; set; }
    
    [Range(1, 5 , ErrorMessage = "Estrelas deve ser entre 1 e 5")]
    public int Estrelas  { get; set; }
    
    [Range(1, 5 , ErrorMessage = "Corações deve ser entre 1 e 5")]
    public int Coracoes  { get; set; }
    
    [Range(1, 5 , ErrorMessage = "Fogos deve ser entre 1 e 5")]
    public int Fogos  { get; set; }
    
    [Range(1, 5 , ErrorMessage = "Humor deve ser entre 1 e 5")]
    public int Humor  { get; set; }
    
    public bool Favorito { get; set; }
    
    public DateTime? DataConclusao { get; set; }
    
    [Required]
    public int UsuarioId { get; set; }
    
    
}
public class LivroUpdateDto
{
    [MaxLength(200)]
    public string? Titulo { get; set; } 
    
    [MaxLength(150)]
    public string? Autor { get; set; }
    
    [MaxLength(500)]
    public string? ImagemUrl { get; set; }
    
    [MaxLength(50)]
    public string? Genero { get; set; }
    
    [Range(1, 365)]
    public int? TempoLeituraDias { get; set; }
    
    [Range(1, 5)]
    public int? Estrelas { get; set; }
    
    [Range(1, 5)]
    public int? Coracoes { get; set; }
    
    [Range(1, 5)]
    public int? Fogos { get; set; }
    
    [Range(1, 5)]
    public int? Humor { get; set; }
    
    public bool? Favorito { get; set; }
    
    public DateTime? DataConclusao { get; set; }
}

public class LivroDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string? ImagemUrl { get; set; }
    public string? Genero { get; set; }
    public int TempoLeituraDias { get; set; }
    public int Estrelas { get; set; }
    public int Coracoes { get; set; }
    public int Fogos { get; set; }
    public int Humor { get; set; }
    public bool Favorito { get; set; }
    public DateTime? DataConclusao { get; set; }
    public int UsuarioId { get; set; }
}
