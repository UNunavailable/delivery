namespace crud_service.Database.Entity
{
    public class Order : DbEntity
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime StorageUntil { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int ProductNum { get; set; }
    }
}
