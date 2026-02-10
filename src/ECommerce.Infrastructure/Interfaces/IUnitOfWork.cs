using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : BaseEntity;
    Task<int> SaveChangesAsync();
}