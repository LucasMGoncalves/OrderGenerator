using Microsoft.Extensions.Options;
using OrderGenerator.Application.Interfaces;
using OrderGenerator.Infrastructure.Configurations;
using OrderGenerator.Application.DTOs;

namespace OrderGenerator.Infrastructure.Services
{
    public class OrderAccumulatorApiService : IOrderAccumulatorApiService
    {
        private readonly IApiClient _apiClient;
        private readonly OrderAccumulatorApiOptions _options;

        public OrderAccumulatorApiService(
            IApiClient apiClient,
            IOptions<OrderAccumulatorApiOptions> options)
        {
            _apiClient = apiClient;
            _options = options.Value;
        }

        public Task<OrderAccumulatorApiResponse> SendOrderFixMessageAsync(
        string fixMessage,
        CancellationToken cancellationToken)
        {
            return _apiClient.PostAsync<OrderAccumulatorApiResponse>(
                _options.ReceiveOrderPath,
                fixMessage,
                "text/plain",
                cancellationToken);
        }
    }
}
