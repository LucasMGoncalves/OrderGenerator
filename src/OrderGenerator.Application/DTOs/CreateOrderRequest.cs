namespace OrderGenerator.Application.DTOs
{
    public class CreateOrderRequest
    {
        public string Symbol { get; init; } = string.Empty;
        public string Side { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public decimal Price { get; init; }
    }
}
