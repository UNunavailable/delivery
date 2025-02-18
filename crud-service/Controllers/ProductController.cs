using crud_service.Database.Entity;
using crud_service.Services;
using delivery.core.Dto;
using Microsoft.AspNetCore.Mvc;

namespace crud_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> Get()
            => await _productService.Get();

        [HttpGet("{id}")]
        public async Task<ProductDto> Get(int id)
            => await _productService.Get(id);

        [HttpPost()]
        public async Task<ProductDto> Post(ProductCreateDto createDto)
            => await _productService.Post(createDto);

        [HttpPut("{id}")]
        public async Task<ProductDto> Put(int id, ProductChangesDto changesDto)
            => await _productService.Put(id, changesDto);

        [HttpDelete("{id}")]
        public async Task<ProductDto> Delete(int id)
            => await _productService.Delete(id);
    }
}
