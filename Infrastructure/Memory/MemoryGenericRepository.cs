using AppCore.Models;
using AppCore.Repositories;
using AppCore.ValueObjects;

namespace Infrastructure.Memory;

public class MemoryGenericRepository<T> : IGenericRepositoryAsync<T>
    where T: EntityBase
{
    protected Dictionary<Guid, T> _data = new();

    public Task<T?> FindByIdAsync(Guid id)
    {
        var result = _data.TryGetValue(id, out var vehicle) ? vehicle : null;
        return Task.FromResult(result);
    }

    public Task<IEnumerable<T>> FindAllAsync()
    {
        var result = _data.Values.ToList().AsEnumerable();
        return Task.FromResult(result);
    }

    public Task<PagedResult<T>> FindPagedAsync(int pageNumber, int pageSize)
    {
        var results = _data.Values.ToList().AsEnumerable();
        var pageResults = results.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        var totalCount = results.Count();

        return Task.FromResult(new PagedResult<T>(pageResults, totalCount, pageNumber, pageSize));
    }

    public Task<T> AddAsync(T entity)
    {
        _data[entity.Id] = entity;
        return Task.FromResult(entity);
    }
    
    public Task<T> UpdateAsync(T entity)
    {
        _data[entity.Id] = entity;
        return Task.FromResult(entity);
    }

    public Task RemoveByIdAsync(Guid id)
    {
        _data.Remove(id);
        return Task.CompletedTask;
    }
}