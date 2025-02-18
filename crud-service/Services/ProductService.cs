using crud_service.Database.Entity;
using delivery.core.Dto;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace crud_service.Services
{
    public class ProductService
    {
        private readonly DbContext _dbContext;
        public ProductService(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductDto> Get(int id)
        {
            var entity = await _dbContext.Set<Product>().FirstOrDefaultAsync(x => x.Article == id)
                ?? throw new NullReferenceException("Объект не найден");
            return entity.Adapt<ProductDto>();
        }

        public async Task<IEnumerable<ProductDto>> Get()
        {
            return await _dbContext.Set<Product>().ProjectToType<ProductDto>().ToListAsync();
        }

        public async Task<ProductDto> Put(int id, ProductChangesDto changes)
        {
            var entity = await _dbContext.Set<Product>().FirstOrDefaultAsync(x => x.Article == id)
                ?? throw new NullReferenceException("Объект не найден");
            entity.Name = changes.Name ?? entity.Name;
            entity.Description = changes.Description ?? entity.Description;
            entity.Price = changes.Price ?? entity.Price;
            entity.StockQuantity = changes.StockQuantity ?? entity.StockQuantity;
            await _dbContext.SaveChangesAsync();
            return entity.Adapt<ProductDto>();
        }

        public async Task<ProductDto> Post(ProductCreateDto newProduct)
        {
            var entity = new Product()
            {
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price,
                StockQuantity = newProduct.StockQuantity
            };
            await _dbContext.Set<Product>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Adapt<ProductDto>();
        }

        public async Task<ProductDto> Delete(int id)
        {
            var entity = await _dbContext.Set<Product>().FirstOrDefaultAsync(x => x.Article == id)
                ?? throw new NullReferenceException("Объект не найден");
            _dbContext.Set<Product>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Adapt<ProductDto>();
        }
    }
}
