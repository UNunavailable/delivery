namespace delivery.core.Dto
{
    public record ProductDto(int Article, string Name, string Description, decimal Price, int StockQuantity);
    public record ProductChangesDto(string? Name, string? Description, decimal? Price, int? StockQuantity);
    public record ProductCreateDto(string Name, string Description, decimal Price, int StockQuantity);
}
