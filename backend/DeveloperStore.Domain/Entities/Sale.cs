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

        private readonly List<SaleItem> _items = new List<SaleItem>();
        public IReadOnlyList<SaleItem> Items => _items.AsReadOnly();
        
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
            _items.Add(item);
            RecalculateTotal();
        }

        public void RemoveItem(SaleItem item)
        {
            _items.Remove(item);
            RecalculateTotal();
        }

        public void RecalculateTotal()
        {
            TotalAmount = _items.Sum(i => i.Total);
        }

        public void Cancel()
        {
            IsCancelled = true;
        }
    }
}

