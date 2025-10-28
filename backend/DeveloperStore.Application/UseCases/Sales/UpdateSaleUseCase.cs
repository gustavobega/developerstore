using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;

namespace DeveloperStore.Application.UseCases.Sales
{
    public class UpdateSaleUseCase
    {
        private readonly ISaleRepository _repository;

        public UpdateSaleUseCase(ISaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Sale> ExecuteAsync(Sale sale)
        {
            var existingSale = await _repository.GetByIdAsync(sale.Id);

            if (existingSale == null)
                throw new InvalidOperationException("Sale not found.");

            existingSale.Customer = sale.Customer;
            existingSale.Branch = sale.Branch;
            existingSale.Date = sale.Date;

            foreach (var oldItem in existingSale.Items.ToList())
                existingSale.RemoveItem(oldItem);

            foreach (var item in sale.Items)
            {
                if (item.Quantity > 20)
                    throw new InvalidOperationException("Cannot sell more than 20 identical items.");

                if (item.Quantity >= 10 && item.Quantity <= 20)
                    item.SetDiscount(0.2m);
                else if (item.Quantity >= 4)
                    item.SetDiscount(0.1m);
                else
                    item.SetDiscount(0);

                item.RecalculateTotal();
                existingSale.AddItem(item);
            }

            existingSale.RecalculateTotal();

            await _repository.UpdateAsync(existingSale);
            await _repository.SaveChangesAsync();

            return existingSale;
        }
    }
}
