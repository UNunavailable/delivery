using crud_service.Database.Entity;
using crud_service.Services;
using delivery.core.Dto;
using Microsoft.AspNetCore.Mvc;

namespace crud_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IEnumerable<OrderDto>> Get(bool? IncludeProducts)
            => await _orderService.Get(IncludeProducts ?? false);

        [HttpGet("{id}")]
        public async Task<OrderDto> Get(int id)
            => await _orderService.Get(id);

        [HttpPost()]
        public async Task<OrderDto> Post(OrderCreateDto createDto)
            => await _orderService.Post(createDto);

        [HttpPut("{id}")]
        public async Task<OrderDto> Put(int id, OrderChangesDto changesDto)
            => await _orderService.Put(id, changesDto);

        [HttpDelete("{id}")]
        public async Task<OrderDto> Delete(int id)
            => await _orderService.Delete(id);
    }
}
