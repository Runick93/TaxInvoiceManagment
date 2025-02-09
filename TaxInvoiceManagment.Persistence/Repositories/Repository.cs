using Microsoft.EntityFrameworkCore;
using TaxInvoiceManagment.Domain.Interfaces;

namespace TaxInvoiceManagment.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TaxInvoiceManagmentDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(TaxInvoiceManagmentDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<int> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            int rowAffected = await _context.SaveChangesAsync();
            return rowAffected;
        }

        public async Task<int> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            int rowAffected = await _context.SaveChangesAsync();
            return rowAffected;
        }

        public async Task<int> DeleteAsync(int id)
        {
            int rowAffected = 0;
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                rowAffected = await _context.SaveChangesAsync();
            }

            return rowAffected;
        }
    }
}
