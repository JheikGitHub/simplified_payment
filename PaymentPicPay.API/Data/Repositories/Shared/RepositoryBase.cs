using Microsoft.EntityFrameworkCore;
using PaymentPicPay.API.Data.Context;
using PaymentPicPay.API.Domain.Models;
using System.Linq.Expressions;

namespace PaymentPicPay.API.Data.Repositories.Shared
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : EntityBase
    {

        private readonly PaymentDataContext _context;
        private readonly DbSet<TEntity> _entitySet;

        public RepositoryBase(PaymentDataContext context)
        {
            _context = context;
            _entitySet = _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
            =>  await _entitySet.ToListAsync();

        public async Task<TEntity> GetAsync(int id, bool AsNoTracking)
        {
            if (AsNoTracking)
                return await _entitySet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return await _entitySet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression)
            => await _entitySet.Where(expression).ToListAsync();
    }
}
