using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShelfAPI.Models;

public class CalendarioMensal
{
    [Key]
    public int Id { get; set; }
    
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int QuantidadeLivros { get; set; } = 0;
    
    [ForeignKey("Usuario")]
    public int UsuarioId { get; set; }

    public Usuario Usuario { get; set; } = null!;

}