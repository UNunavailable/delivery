using business_service.Services;

namespace business_service.Extension
{
    public static class DbService
    {
        public static void ConfigureDbServices(this IServiceCollection services)
        {
            services.AddHttpClient<OrderClientService>((provider, client) =>
            {
                var url = provider.GetRequiredService<IConfiguration>().GetValue<string>("DbServiceUrl")
                    ?? throw new NullReferenceException("Объявите путь до сервиса базы данных");
                client.BaseAddress = new Uri(url);
            });
            services.AddHttpClient<ProductClientService>((provider, client) =>
            {
                var url = provider.GetRequiredService<IConfiguration>().GetValue<string>("DbServiceUrl")
                    ?? throw new NullReferenceException("Объявите путь до сервиса базы данных");
                client.BaseAddress = new Uri(url);
            });
        }
    }
}
