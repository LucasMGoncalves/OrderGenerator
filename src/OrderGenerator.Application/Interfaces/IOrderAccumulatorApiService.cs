using OrderGenerator.Application.DTOs;

namespace OrderGenerator.Application.Interfaces
{
    public interface IOrderAccumulatorApiService
    {
        Task<OrderAccumulatorApiResponse> SendOrderFixMessageAsync(
            string fixMessage,
            CancellationToken cancellationToken);
    }
}
