using delivery.core.Dto;
using System.Net.Http;

namespace business_service.Services
{
    public class ProductClientService
    {
        private readonly HttpClient _httpClient;
        private readonly string CONTROLLER_PATH = "/api/Product";

        public ProductClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDto> GetProductAsync(int productId)
        {
            return await _httpClient.GetFromJsonAsync<ProductDto>($"{CONTROLLER_PATH}/{productId}")
                ?? throw new NullReferenceException("Not found!");
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>($"{CONTROLLER_PATH}/")
                ?? throw new NullReferenceException("Not found!");
        }

        public async Task<ProductDto> UpdateProductAsync(int productId, ProductChangesDto changesDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{CONTROLLER_PATH}/{productId}", changesDto);
            return await response.Content.ReadFromJsonAsync<ProductDto>();
        }

        public async Task<ProductDto> CreateProductAsync(ProductCreateDto createDto)
        {
            var response = await _httpClient.PostAsJsonAsync(CONTROLLER_PATH, createDto);
            return await response.Content.ReadFromJsonAsync<ProductDto>();
        }

        public async Task DeleteProductAsync(int productId)
        {
            await _httpClient.DeleteAsync($"{CONTROLLER_PATH}/{productId}");
        }
    }
}
