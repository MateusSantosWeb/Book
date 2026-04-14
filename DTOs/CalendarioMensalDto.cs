using System.ComponentModel.DataAnnotations;

namespace BookShelfAPI.DTOs;

public class CalendarioMensalCreateDto
{
    [Required]
    [Range(2020, 2100)]
    public int Ano { get; set; }
    
    [Required]
    [Range(1, 12, ErrorMessage = "Mês Deve ser entre 1 e 12")]
    public int Mes { get; set; }
    
    [Required]
    [Range(0, 1000)]
    public int QuantidadeLivros { get; set; }

    [Required]
    public int UsuarioId { get; set; }
    
}

public class CalendarioMensalUpdateDto
{
    [Range(0, 1000)]
    public int QuantidadeLivros { get; set; }
    
}

public class CalendarioMensalDto
{
    public int Id { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public string NomeMes { get; set; } = string.Empty;
    public int QuantidadeLivros { get; set; }
    public int UsuarioId { get; set; }
}

public class CalendarioAnualDto
{
    public int Ano { get; set; }
    public List<CalendarioMensalDto> Meses { get; set; } = new();
    public int TotalLivrosAno { get; set; }
    public string MesMaisLido { get; set; } = string.Empty;
}
