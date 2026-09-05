using Microsoft.Extensions.DependencyInjection;
using OrderGenerator.Application.Interfaces;
using OrderGenerator.Infrastructure.Services;

namespace OrderGenerator.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddScoped<ISymbolService, SymbolService>();
            services.AddScoped<IFixOrderMessageService, FixOrderMessageService>();

            return services;
        }
    }
}
