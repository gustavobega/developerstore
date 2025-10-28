using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;

namespace DeveloperStore.Application.UseCases.Sales
{
    public class GetSaleByIdUseCase
    {
        private readonly ISaleRepository _repository;

        public GetSaleByIdUseCase(ISaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Sale?> ExecuteAsync(Guid saleId)
        {
            return await _repository.GetByIdAsync(saleId);
        }
    }
}
