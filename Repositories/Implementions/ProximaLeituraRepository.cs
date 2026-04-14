using BookShelfAPI.Data;
using BookShelfAPI.Models;
using BookShelfAPI.Repositories.Interfaces;

namespace BookShelfAPI.Repositories.Implementions;

public class ProximaLeituraRepository : Repository<ProximoLeitura>,IProximaLeitura
{
    public ProximaLeituraRepository(BookShelfContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProximoLeitura>> GetAllAsync()
    {
        return await base.GetAllAsync();
    }

    public new async Task<ProximoLeitura?> GetByIdAsync(int id)
    {
        return await base.GetByIdAsync(id);
    }

    public new async Task AddAsync(ProximoLeitura proximoLeitura)
    {
        await base.AddAsync(proximoLeitura);
    }

    public new async Task UpdateAsync(ProximoLeitura proximoLeitura)
    {
        await base.UpdateAsync(proximoLeitura);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await base.GetByIdAsync(id);
        if (entity is null)
        {
            return;
        }

        await base.DeleteAsync(entity);
    }
}
