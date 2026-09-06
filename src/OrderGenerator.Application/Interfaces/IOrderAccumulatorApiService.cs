using OrderGenerator.Application.DTOs;

namespace OrderGenerator.Application.Interfaces
{
    public interface IOrderAccumulatorApiService
    {
        Task<ExecutionReportResponse> SendOrderFixMessageAsync(string fixMessage, CancellationToken cancellationToken);
    }
}
