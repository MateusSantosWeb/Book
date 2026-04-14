using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShelfAPI.Models;

public class Livro
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } =  string.Empty;
    
    [Required]
    [MaxLength(150)]
    public string Autor { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? ImageUrl { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Genero { get; set; }
    
    public int TempoDeLeiturasDias { get; set; }
    
    public int Estrelas { get; set; }
    public int Coracoes { get; set; }
    public int Fogos { get; set; }
    public int Humor  { get; set; }
    
    public bool Favoarito { get; set; }
    
    public DateTime DataConclusao { get; set; }
    
    [ForeignKey("Usuario")]
    public int  UsuarioId { get; set; }

    public Usuario Usuario { get; set; } = null!;


}