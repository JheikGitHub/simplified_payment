using PaymentPicPay.API.Domain.Models;
using System.Linq.Expressions;

namespace PaymentPicPay.API.Data.Repositories.Shared
{
    public interface IRepositoryBase<TEntity> : IDisposable 
        where TEntity : EntityBase
    {
        Task<TEntity> GetAsync(int id, bool AsNoTracking);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression);
        Task AddAsync(TEntity entity);

    }
}
