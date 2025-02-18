using crud_service.Database.Entity;
using delivery.core.Dto;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace crud_service.Services
{
    public class OrderService
    {
        private readonly DbContext _dbContext;
        public OrderService(DbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<OrderDto> Get(int id)
        {
            var entity = _dbContext.Set<Order>()
                .Include(order => order.Product)
                .Where(x => x.Id == id)
                .ProjectToType<OrderDto>()
                .FirstOrDefault()
                ?? throw new NullReferenceException("Объект не найден");
            return entity;
        }

        public async Task<IEnumerable<OrderDto>> Get(bool IncludeProducts = false)
        {
            var set = _dbContext.Set<Order>();
            if(IncludeProducts) 
                set.Include(order => order.Product);
            return await set.ProjectToType<OrderDto>().ToListAsync();
        }

        public async Task<OrderDto> Put(int id, OrderChangesDto changes)
        {
            var entity = await _dbContext.Set<Order>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync()
                ?? throw new NullReferenceException("Объект не найден");
            entity.OrderDate = changes.OrderDate ?? entity.OrderDate;
            entity.StorageUntil = changes.StorageUntil ?? entity.StorageUntil;
            entity.Status = changes.Status ?? entity.Status;
            entity.CustomerName = changes.CustomerName ?? entity.CustomerName;
            entity.PhoneNumber = changes.PhoneNumber ?? entity.PhoneNumber;
            entity.TotalPrice = changes.TotalPrice ?? entity.TotalPrice;
            entity.ProductId = changes.ProductId ?? entity.ProductId;
            entity.ProductNum = changes.ProductNum ?? entity.ProductNum;
            await _dbContext.SaveChangesAsync();
            return entity.Adapt<OrderDto>();
        }

        public async Task<OrderDto> Post(OrderCreateDto newOrder)
        {
            var entity = new Order()
            {
                OrderDate = DateTime.Now,
                StorageUntil = newOrder.StorageUntil,
                Status = newOrder.Status,
                CustomerName = newOrder.CustomerName,
                PhoneNumber = newOrder.PhoneNumber,
                TotalPrice = newOrder.TotalPrice,
                ProductId = newOrder.ProductId,
                ProductNum = newOrder.ProductNum
            };
            await _dbContext.Set<Order>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            var dto = await _dbContext.Set<Order>()
                .Include(order => order.Product)
                .Where(x => x.Id == entity.Id)
                .ProjectToType<OrderDto>()
                .FirstOrDefaultAsync()
                ?? entity.Adapt<OrderDto>();
            return dto;
        }

        public async Task<OrderDto> Delete(int id)
        {
            var entity = await _dbContext.Set<Order>().FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new NullReferenceException("Объект не найден");
            _dbContext.Set<Order>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Adapt<OrderDto>();
        }
    }
}
