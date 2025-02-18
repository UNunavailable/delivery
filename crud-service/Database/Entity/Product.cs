namespace crud_service.Database.Entity
{
    public class Product : DbEntity
    {
        public int Article { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public IEnumerable<Order> Orders { get; set; } = [];
    }
}
