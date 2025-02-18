using business_service.Services;
using delivery.core.Dto;
using Microsoft.AspNetCore.Mvc;

namespace business_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductClientService _productClientService;

        public ProductController(ProductClientService productClientService)
        {
            _productClientService = productClientService;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> Get()
            => await _productClientService.GetProductsAsync();
    }
}
