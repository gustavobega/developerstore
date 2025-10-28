using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;

namespace DeveloperStore.Application.UseCases.Sales
{
    public class CancelSaleUseCase
    {
        private readonly ISaleRepository _repository;

        public CancelSaleUseCase(ISaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Sale> ExecuteAsync(Guid saleId)
        {
            var existingSale = await _repository.GetByIdAsync(saleId);

            if (existingSale == null)
                throw new InvalidOperationException("Sale not found.");

            existingSale.Cancel();

            await _repository.UpdateAsync(existingSale);
            await _repository.SaveChangesAsync();

            return existingSale;
        }
    }
}
