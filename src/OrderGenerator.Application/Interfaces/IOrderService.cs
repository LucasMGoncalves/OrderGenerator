using OrderGenerator.Application.DTOs;

namespace OrderGenerator.Application.Interfaces
{
    public interface IOrderService
    {
        Task<CreateOrderResponse> CreateOrderAsync(
            CreateOrderRequest request,
            CancellationToken cancellationToken);
    }
}
