using AppCore.Models;
using AppCore.Repositories;
using AppCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfGenericRepository<T> : IGenericRepositoryAsync<T>
	where T : EntityBase
{
	protected readonly DbSet<T> Set;

	public EfGenericRepository(DbSet<T> set)
	{
		Set = set;
	}

	public async Task<T?> FindByIdAsync(Guid id)
	{
		return await Set.FindAsync(id);
	}

	public async Task<IEnumerable<T>> FindAllAsync()
	{
		return await Set.AsNoTracking().ToListAsync();
	}

	public async Task<PagedResult<T>> FindPagedAsync(int pageNumber, int pageSize)
	{
		var query = Set.AsNoTracking();
		var totalCount = await query.CountAsync();
		var items = await query
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync();

		return new PagedResult<T>(items, totalCount, pageNumber, pageSize);
	}

	public async Task<T> AddAsync(T entity)
	{
		var entry = await Set.AddAsync(entity);
		return entry.Entity;
	}

	public Task<T> UpdateAsync(T entity)
	{
		var existing = Set.Find(entity.Id);
		if (existing is null)
		{
			throw new KeyNotFoundException($"{typeof(T).Name} with id={entity.Id} was not found.");
		}

		var entityEntry = Set.Update(entity);
		return Task.FromResult(entityEntry.Entity);
	}

	public async Task RemoveByIdAsync(Guid id)
	{
		var entity = await Set.FindAsync(id);
		if (entity is null)
		{
			throw new KeyNotFoundException($"{typeof(T).Name} with id={id} was not found.");
		}

		Set.Remove(entity);
	}
}


