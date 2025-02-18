namespace delivery.core.Dto
{
    public record OrderDto(int Id, DateTime StorageUntil, string Status, string CustomerName, string PhoneNumber, decimal TotalPrice, ProductDto? Product, int ProductNum);
    public record OrderChangesDto(DateTime? OrderDate, DateTime? StorageUntil, string? Status, string? CustomerName, string? PhoneNumber, decimal? TotalPrice, int? ProductId, int? ProductNum);
    public record OrderCreateDto(DateTime StorageUntil, string Status, string CustomerName, string PhoneNumber, decimal TotalPrice, int ProductId, int ProductNum);
}
