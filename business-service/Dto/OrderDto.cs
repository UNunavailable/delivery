namespace business_service.Dto
{
    /// <summary>
    /// Форма добавления заказа
    /// </summary>
    /// <param name="CustomerName">Имя покупателя</param>
    /// <param name="PhoneNumber">Номер телефона покупателя в формате</param>
    /// <param name="ProductArticle"></param>
    /// <param name="ProductNum"></param>
    public record AddOrderDto(string CustomerName, string PhoneNumber, int ProductArticle, uint ProductNum);
}
