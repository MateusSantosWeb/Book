using System.ComponentModel.DataAnnotations;

namespace BookShelfAPI.Models;

public class Usuario
{
    [Key]
    public int Id { get; set; }
    
    
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } =  string.Empty;
    
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public ICollection<Livro> Livro { get; set; } = new List<Livro>();
    public MetaLeitura? MetaLeitura { get; set; }
    public DesafioAZ? DesafioAZ { get; set; }
    public ICollection<CalendarioMensal> CalendariosMensais { get; set; } = new List<CalendarioMensal>();

}
