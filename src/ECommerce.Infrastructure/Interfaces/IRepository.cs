using System.Linq.Expressions;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    // Queries
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    // Commands
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity); // Soft Delete
}