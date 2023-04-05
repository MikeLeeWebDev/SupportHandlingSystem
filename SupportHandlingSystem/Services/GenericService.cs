using Microsoft.EntityFrameworkCore;
using SupportHandlingSystem.Contexts;
using System.Linq.Expressions;

namespace SupportHandlingSystem.Services;

internal abstract class GenericService<TEntity> where TEntity : class
{
    private readonly DataContext _context = new DataContext();

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var item = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        return item ?? null!;
    }

    public virtual async Task<TEntity> SaveAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate)
    {
        var item = await GetAsync(predicate);
        if (item == null)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        return item;
    }
}


