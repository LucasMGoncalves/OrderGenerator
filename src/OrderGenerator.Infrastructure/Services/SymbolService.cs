using OrderGenerator.Application.DTOs;
using OrderGenerator.Application.Interfaces;

namespace OrderGenerator.Infrastructure.Services
{
    public class SymbolService : ISymbolService
    {
        private static readonly IReadOnlyCollection<SymbolResponse> Symbols =
        [
            new() { Code = "PETR4" },
            new() { Code = "VALE3" },
            new() { Code = "VIIA4" }
        ];

        public Task<IReadOnlyCollection<SymbolResponse>> GetSymbolsAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Symbols);
        }

        public Task<bool> IsValidAsync(
        string code,
        CancellationToken cancellationToken)
        {
            var valid = Symbols.Any(
                symbol => string.Equals(symbol.Code, code)
            );

            return Task.FromResult(valid);
        }
    }
}
