using System.ComponentModel.DataAnnotations;

namespace BookShelfAPI.Models;

public class ProximoLeitura
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Autor { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? ImageUrl{ get; set; }
    
    [MaxLength(500)]
    public string? Complemento { get; set; }
    
    public int Prioridade { get; set; }
    
    
}