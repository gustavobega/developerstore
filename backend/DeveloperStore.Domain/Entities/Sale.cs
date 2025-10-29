namespace DeveloperStore.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string SaleNumber { get; private set; }
        public string Customer { get; set; }
        public string Branch { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; private set; } = false;

        public List<SaleItem> Items { get; set; } = new List<SaleItem>();
        
        public Sale(string saleNumber, string customer, string branch, DateTime date)
        {
            SaleNumber = saleNumber;
            Customer = customer;
            Branch = branch;
            Date = date;
        }

        public Sale(Guid id, string saleNumber, string customer, string branch, DateTime date)
        {
            Id = id; 
            SaleNumber = saleNumber;
            Customer = customer;
            Branch = branch;
            Date = date;
        }

        public void AddItem(SaleItem item)
        {
            Items.Add(item);
            RecalculateTotal();
        }

        public void RemoveItem(SaleItem item)
        {
            Items.Remove(item);
            RecalculateTotal();
        }

        public void RecalculateTotal()
        {
            TotalAmount = Items.Sum(i => i.Total);
        }

        public void Cancel()
        {
            IsCancelled = true;
        }
    }
}

