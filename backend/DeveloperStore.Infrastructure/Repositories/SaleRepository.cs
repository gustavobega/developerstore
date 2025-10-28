using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DeveloperStoreDbContext _dbContext;

        public SaleRepository(DeveloperStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Sale sale)
        {
            await _dbContext.Sales.AddAsync(sale);
        }

        public async Task<Sale?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _dbContext.Sales
                .Include(s => s.Items)
                .ToListAsync();
        }

        public async Task UpdateAsync(Sale sale)
        {
            _dbContext.Sales.Update(sale);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var sale = await GetByIdAsync(id);
            if (sale != null)
                _dbContext.Sales.Remove(sale);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
