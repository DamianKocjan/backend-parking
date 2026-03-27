using AppCore.Models;
using AppCore.ValueObjects;

namespace AppCore.Repositories;

public interface IGenericRepositoryAsync<T> where T : EntityBase
{
    Task<T?> FindByIdAsync(Guid id);
    Task<IEnumerable<T>> FindAllAsync();
    Task<PagedResult<T>> FindPagedAsync(int pageNumber, int pageSize);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task RemoveByIdAsync(Guid id);
}