using BookShelfAPI.DTOs;

namespace BookShelfAPI.Services.Interfaces;

public interface IMetaLeituraService
{
    Task<MetaLeituraComLivrosDto?> GetMetaPorUsuarioAsync(int usuarioId, int? ano);
    Task<MetaDeLeituraDto> CreateMetaAsync(MetaDeLeituraCreayeDto dto);
    Task<bool> UpdateMetaAsync(int id, MetaDeLeituraUpdateDto dto);
    Task<bool> DeleteMetaAsync(int id);
    Task<bool> AdicionarLivroNaMetaAsync(int metaId, int livroId);
    Task<bool> RemoverLivroDaMetaAsync(int metaId, int livroId);
}
