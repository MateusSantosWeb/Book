using System.ComponentModel.DataAnnotations;

namespace BookShelfAPI.DTOs;

public class ProximaLeituraDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Titulo Obrigatório")]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? Autor { get; set; }
    
    [MaxLength(500)]
    public string? ImageUrl { get; set; }
    
    [MaxLength(500)]
    public string? Complemento { get; set; }
    
    public int Prioridade { get; set; }
    
    
}
