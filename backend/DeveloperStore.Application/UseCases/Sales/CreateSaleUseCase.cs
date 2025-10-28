using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;

namespace DeveloperStore.Application.UseCases.Sales
{
    public class CreateSaleUseCase
    {
        private readonly ISaleRepository _repository;

        public CreateSaleUseCase(ISaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Sale> ExecuteAsync(Sale sale)
        {
            foreach (var item in sale.Items)
            {
                if (item.Quantity > 20)
                    throw new InvalidOperationException("Não é possível vender mais de 20 itens iguais.");

                if (item.Quantity >= 10 && item.Quantity <= 20)
                    item.SetDiscount(0.2m);
                else if (item.Quantity >= 4)
                    item.SetDiscount(0.1m);
                else
                    item.SetDiscount(0);

                item.RecalculateTotal();
            }

            sale.RecalculateTotal();

            await _repository.AddAsync(sale);
            await _repository.SaveChangesAsync();

            return sale;
        }
    }
}
