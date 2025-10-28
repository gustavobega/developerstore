using System.Text.Json.Serialization;

namespace DeveloperStore.Domain.Entities
{
    public class SaleItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Product { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public Guid SaleId { get; set; } 
        
        [JsonIgnore]
        public Sale? Sale { get; set; }

        public SaleItem(string product, int quantity, decimal unitPrice)
        {
            Product = product;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public void SetDiscount(decimal discount)
        {
            Discount = discount;
        }

        public void RecalculateTotal()
        {
            Total = UnitPrice * Quantity * (1 - Discount);
        }
    }
}
