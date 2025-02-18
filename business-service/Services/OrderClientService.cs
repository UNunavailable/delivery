using delivery.core.Dto;

namespace business_service.Services
{
    public class OrderClientService
    {
        private readonly HttpClient _httpClient;
        private readonly string CONTROLLER_PATH = "/api/Order";

        public OrderClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OrderDto> GetOrderAsync(int orderId)
        {
            return await _httpClient.GetFromJsonAsync<OrderDto>($"{CONTROLLER_PATH}/{orderId}")
                ?? throw new NullReferenceException("Not found!");
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync(bool IncludeProducts = true)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<OrderDto>>($"{CONTROLLER_PATH}?{nameof(IncludeProducts)}={IncludeProducts}")
                ?? throw new NullReferenceException("Not found!");
        }

        public async Task<OrderDto> UpdateOrderAsync(int orderId, OrderChangesDto changesDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{CONTROLLER_PATH}/{orderId}", changesDto);
            return await response.Content.ReadFromJsonAsync<OrderDto>();
        }

        public async Task<OrderDto> CreateOrderAsync(OrderCreateDto createDto)
        {
            var response = await _httpClient.PostAsJsonAsync(CONTROLLER_PATH, createDto);
            return await response.Content.ReadFromJsonAsync<OrderDto>();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            await _httpClient.DeleteAsync($"{CONTROLLER_PATH}/{orderId}");
        }
    }
}
