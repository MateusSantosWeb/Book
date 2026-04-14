using System.ComponentModel.DataAnnotations;

namespace BookShelfAPI.DTOs;

public class UsuarioCreateDto
{
    [Required(ErrorMessage = "Nome é Obrigatório")]
    [MaxLength(100, ErrorMessage = "Nome Deve ter no maximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;
    
}

public class UsuarioDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}
