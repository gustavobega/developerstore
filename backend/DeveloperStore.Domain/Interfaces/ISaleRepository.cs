using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Interfaces
{
    public interface ISaleRepository
    {
        Task AddAsync(Sale sale);
        Task<Sale?> GetByIdAsync(Guid id);
        Task<IEnumerable<Sale>> GetAllAsync();
        Task UpdateAsync(Sale sale);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}
