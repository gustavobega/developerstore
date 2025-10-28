namespace DeveloperStore.API.DTOs
{
    public class SaleDto
    {
        public Guid? Id { get; set; }
        public string SaleNumber { get; set; }
        public string Customer { get; set; }
        public string Branch { get; set; }
        public DateTime Date { get; set; }
        public List<SaleItemDto> Items { get; set; } = new();
    }
}