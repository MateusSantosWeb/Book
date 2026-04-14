using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShelfAPI.Models;

public class MetaLeitura
{
    [Key]
    public int Id { get; set; }
    
    public int Ano { get; set; }
    
    public int QuantidadeObjetivo { get; set; }
    
    public int QuantidadeLida { get; set; }
    
    
    [ForeignKey("Usuario")]
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    
    public ICollection<MetaLeituraLivro> LivrosNaMeta { get; set; } = new List<MetaLeituraLivro>();
    
}

public class MetaLeituraLivro
{
    [ForeignKey("MetaLeitura")]
    public int MetaLeituraId { get; set; }

    public MetaLeitura MetaLeitura { get; set; } = null!;
    
    [ForeignKey("Livro")]
    public int LivroId { get; set; }
    public Livro Livro { get; set; } = null!;
    
    public DateTime DataAdicao { get; set; } = DateTime.UtcNow;
}
