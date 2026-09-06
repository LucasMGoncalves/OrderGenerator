using OrderGenerator.Domain.Entities;

namespace OrderGenerator.Application.Interfaces
{
    public interface ISymbolService
    {
        Task<IReadOnlyCollection<Symbol>> GetSymbolsAsync(CancellationToken cancellationToken);

        Task<bool> IsValidAsync(string code, CancellationToken cancellationToken);
    }
}
