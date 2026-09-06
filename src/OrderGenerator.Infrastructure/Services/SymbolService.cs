using OrderGenerator.Application.Interfaces;
using OrderGenerator.Domain.Entities;

namespace OrderGenerator.Infrastructure.Services
{
    public class SymbolService : ISymbolService
    {
        private static readonly IReadOnlyCollection<Symbol> Symbols =
        [
            new Symbol("PETR4"),
            new Symbol("VALE3"),
            new Symbol("VIIA4")
        ];

        public Task<IReadOnlyCollection<Symbol>> GetSymbolsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Symbols);
        }

        public Task<bool> IsValidAsync(string code, CancellationToken cancellationToken)
        {
            var valid = Symbols.Any(symbol => string.Equals(symbol.Code, code));

            return Task.FromResult(valid);
        }
    }
}
