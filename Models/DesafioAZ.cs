using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShelfAPI.Models;

public class DesafioAZ
{
    public int Id { get; set; }
    
    public int Ano { get; set; }
    
    [ForeignKey("Usuario")]
    public int UsuarioId { get; set; }

    public Usuario Usuario { get; set; } = null!;
    
    public ICollection<LetraDesafio> Letras { get; set; } = new List<LetraDesafio>();
    
}

public class LetraDesafio
{
    [Key] public int Id { get; set; }

    [Required] [MaxLength(1)] public string Letra { get; set; } = string.Empty;

    [MaxLength(200)] public string? TituloLivro { get; set; }

    public bool Completado { get; set; } = false;

    [ForeignKey("DesafioAZ")] public int DesafioAZId { get; set; }

    public DesafioAZ? DesafioAZ { get; set; } = null!;

}  