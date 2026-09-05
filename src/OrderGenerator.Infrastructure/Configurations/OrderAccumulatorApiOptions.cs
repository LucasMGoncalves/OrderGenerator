namespace OrderGenerator.Infrastructure.Configurations
{
    public class RemoteApisOptions
    {
        public OrderAccumulatorApiOptions OrderAccumulator { get; init; } = new();
    }

    public class OrderAccumulatorApiOptions
    {
        public string BaseUrl { get; init; } = string.Empty;
        public string ReceiveOrderPath { get; init; } = string.Empty;
        public int TimeoutSeconds { get; init; } = 5;
    }
}
