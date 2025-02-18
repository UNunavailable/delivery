using business_service.Dto;
using business_service.Services;
using delivery.core.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace business_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderClientService _orderClientService;
        private readonly ProductClientService _productClientService;
        private readonly TimeSpan StorageTime = new(7, 0, 0, 0);

        public OrderController(OrderClientService orderClientService, ProductClientService productClientService)
        {
            _orderClientService = orderClientService;
            _productClientService = productClientService;
        }

        /// <summary>
        /// Получение всех заказов с товарами
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<OrderDto>> Get()
            => await _orderClientService.GetOrdersAsync();

        /// <summary>
        /// Получение заказа с товарами по артикулу
        /// </summary>
        /// <param name="article">Идентификатор товара</param>
        /// <returns></returns>
        [HttpGet("{article}")]
        public async Task<IEnumerable<OrderDto>> Get(int article)
        {
            var orders = await _orderClientService.GetOrdersAsync();
            return orders
                .Where(order => order.Product.Article == article)
                .ToList();
        }

        /// <summary>
        /// Создание заказа
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public async Task<OrderDto> Create(AddOrderDto createDto)
        {
            var pattern = "\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}";
            if (!Regex.IsMatch(createDto.PhoneNumber, pattern))
                throw new ArgumentException("Номер введён неверно.", nameof(createDto.PhoneNumber));

            ProductDto product;
            try
            {
                product = await _productClientService.GetProductAsync(createDto.ProductArticle);
            }
            catch (HttpRequestException) 
            {
                throw new ArgumentException($"Товар с артиклем({createDto.ProductArticle}) не найден.", nameof(createDto.ProductArticle));
            }

            if(product.StockQuantity < createDto.ProductNum)
                throw new Exception($"Товара с артиклем({createDto.ProductArticle}) недостаточно на складе.");

            _ = _productClientService.UpdateProductAsync(product.Article, new ProductChangesDto(
                null, null, null, StockQuantity: product.StockQuantity - (int)createDto.ProductNum));

            OrderDto newOrder;
            try
            {
                newOrder = await _orderClientService.CreateOrderAsync(new OrderCreateDto(
                    DateTime.Now + StorageTime,
                    "Created",
                    createDto.CustomerName,
                    createDto.PhoneNumber,
                    product.Price * createDto.ProductNum,
                    product.Article,
                    (int)createDto.ProductNum
                    ));
            }
            catch (HttpRequestException)
            {
                throw new Exception("Не удалось создать заказ");
            }
            return newOrder;
        }

        /// <summary>
        /// Изменение статуса заказа
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <param name="newStatus">Новый статус</param>
        /// <returns></returns>
        [HttpPut("{id}/status/{newStatus}")]
        public async Task<OrderDto> ChangeOrderStatus(int id, string newStatus)
        {
            OrderDto order;
            try
            {
                order = await _orderClientService.GetOrderAsync(id);
            }
            catch (HttpRequestException)
            {
                throw new Exception("Заказ не найден");
            }

            OrderDto updatedOrder;
            try
            {
                updatedOrder = await _orderClientService.UpdateOrderAsync(id, new OrderChangesDto(
                    null, null,
                    Status: newStatus,
                    null, null, null, null, null));
            }
            catch (HttpRequestException)
            {
                throw new Exception("Не удалось обновить заказ");
            }
            return updatedOrder;
        }

        /// <summary>
        /// Добавление/удаление товара 
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <param name="ProductArticle">Артикль товара</param>
        /// <param name="ProductNum">Количество товара</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        [HttpPost("{id}/change-products/")]
        public async Task<OrderDto> ChangeProducts(int id, int ProductArticle, int ProductNum)
        {
            OrderDto order;
            try
            {
                order = await _orderClientService.GetOrderAsync(id);
            }
            catch (HttpRequestException)
            {
                throw new Exception("Заказ не найден");
            }

            OrderDto newOrder;
            if(order.Product.Article == ProductArticle)
            {
                if(order.Product.StockQuantity + order.ProductNum < ProductNum)
                    throw new Exception($"Товара с артиклем({ProductArticle}) недостаточно на складе.");

                _ = _productClientService.UpdateProductAsync(order.Product.Article, new ProductChangesDto(
                    null, null, null, StockQuantity: order.Product.StockQuantity + order.ProductNum - ProductNum));

                try
                {
                    newOrder = await _orderClientService.UpdateOrderAsync(order.Id, new OrderChangesDto(null, null, null, null, null,
                        TotalPrice: order.Product.Price * ProductNum, null, ProductNum: ProductNum));
                }
                catch (HttpRequestException)
                {
                    throw new Exception("Не удалось обновить заказ");
                }
            } else
            {
                ProductDto newProduct;
                try
                {
                    newProduct = await _productClientService.GetProductAsync(id)
                        ?? throw new ArgumentException($"Товар с артиклем({ProductArticle}) не найден.", nameof(ProductArticle));
                }
                catch (HttpRequestException)
                {
                    throw new ArgumentException($"Товар с артиклем({ProductArticle}) не найден.", nameof(ProductArticle));
                }
                if(newProduct.StockQuantity < ProductNum)
                    throw new Exception($"Товара с артиклем({ProductArticle}) недостаточно на складе.");

                _ = _productClientService.UpdateProductAsync(newProduct.Article, new ProductChangesDto(
                    null, null, null, StockQuantity: newProduct.StockQuantity - ProductNum));
                _ = _productClientService.UpdateProductAsync(order.Product.Article, new ProductChangesDto(
                    null, null, null, StockQuantity: order.Product.Article + order.ProductNum));

                try
                {
                    newOrder = await _orderClientService.UpdateOrderAsync(order.Id, new OrderChangesDto(null, null, null, null, null,
                        TotalPrice: newProduct.Price * ProductNum, ProductId: newProduct.Article, ProductNum: ProductNum));
                }
                catch (HttpRequestException)
                {
                    throw new Exception("Не удалось обновить заказ");
                }
            }
            return newOrder;
        }

        [HttpPost("{id}/cancel")]
        public async Task<OrderDto> Cancel(int id)
        {
            OrderDto order;
            try
            {
                order = await _orderClientService.GetOrderAsync(id);
                _ = _productClientService.UpdateProductAsync(order.Product!.Article, new ProductChangesDto(null, null, null, order.ProductNum + order.Product!.StockQuantity));
                _ = _orderClientService.DeleteOrderAsync(order.Id);
            }
            catch (HttpRequestException)
            {
                throw new Exception("Заказ не найден");
            }
            return order;
        }
    }
}
