using DeveloperStore.Domain.Entities;
using DeveloperStore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Application.UseCases.Sales
{
    public class GetAllSalesUseCase
    {
        private readonly DeveloperStoreDbContext _dbContext;

        public GetAllSalesUseCase(DeveloperStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Sale>> ExecuteAsync()
        {
            return await _dbContext.Sales
                .Include(s => s.Items)
                .ToListAsync();
        }
    }
}
