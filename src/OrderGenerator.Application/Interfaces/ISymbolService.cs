using OrderGenerator.Application.DTOs;

namespace OrderGenerator.Application.Interfaces
{
    public interface ISymbolService
    {
        Task<IReadOnlyCollection<SymbolResponse>> GetSymbolsAsync(
            CancellationToken cancellationToken);

        Task<bool> IsValidAsync(
            string code,
            CancellationToken cancellationToken);
    }
}
