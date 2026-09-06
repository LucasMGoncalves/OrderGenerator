using OrderGenerator.Domain.Enum;

namespace OrderGenerator.Domain.Entities
{
    public class Order
    {
        public string Symbol { get; }
        public OrderSide Side { get; }
        public decimal Amount { get; }
        public decimal Price { get; }

        public Order(
            string symbol,
            OrderSide side,
            decimal amount,
            decimal price)
        {
            Symbol = symbol;
            Side = side;
            Amount = amount;
            Price = price;
        }

        public string? Validate()
        {
            if (string.IsNullOrWhiteSpace(Symbol))
            {
                return "Simbolo é obrigatório.";
            }

            if (
                Amount <= 0 ||
                Amount >= 100_000 ||
                Amount != decimal.Truncate(Amount)
            )
            {
                return "Quantidade deve ser maior que 0.";
            }

            if (
                Price <= 0 ||
                Price >= 1000 ||
                decimal.Round(Price, 2) != Price
            )
            {
                return "Preço deve estar entre 0.01 e 999.99.";
            }

            return null;
        }
    }
}
