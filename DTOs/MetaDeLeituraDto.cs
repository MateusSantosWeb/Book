using System.ComponentModel.DataAnnotations;

namespace BookShelfAPI.DTOs;

public class MetaDeLeituraCreayeDto
{
    [Required]
    [Range(2020, 2100)]
    public int Ano { get; set; }
    
    [Required]
    [Range(1, 1000, ErrorMessage = "A quantidade objetivo deve ser entre 1 a 1000")]
    public int QuantidadeObejetivo { get; set; }
    
    [Required]
    public int UsuarioId { get; set; }
    
}
    


public class MetaDeLeituraUpdateDto
{
    [Range(1,1000)]
    public int? QuantidadeObejetivo { get; set; }
    
}

public class MetaDeLeituraDto
{
    public int Id { get; set; }
    public int Ano { get; set; }
    public int QuantidadeObjetivo { get; set; }
    public int QuantidadeLida { get; set; }
    public int UsuarioId { get; set; }
    public double Progresso => QuantidadeObjetivo > 0
        ? Math.Round((double)QuantidadeLida / QuantidadeObjetivo * 100, 2)
        : 0;

}

public class AdicionarLivroMetaDto
{
    [Required]
    public int LivroId { get; set; }
    
}

public class MetaLeituraComLivrosDto : MetaDeLeituraCreayeDto
{
    public int Id { get; set; }
    public int QuantidadeLida { get; set; }
    public List<LivroDto> Livros { get; set; } = new();
    public Dictionary<string, int> GenerosMaisLidos { get; set; } = new();
    public List<GeneroContagemDto> Top3Generos { get; set; } = new();
}

public class GeneroContagemDto
{
    public string Nome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
}
