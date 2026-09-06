using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderGenerator.Application.Interfaces;
using OrderGenerator.Infrastructure.Configurations;
using OrderGenerator.Infrastructure.Integrations;
using OrderGenerator.Infrastructure.Services;

namespace OrderGenerator.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<OrderAccumulatorApiOptions>(configuration.GetSection("OrderAccumulatorApi"));

            services.AddScoped<ISymbolService, SymbolService>();
            services.AddScoped<IFixOrderMessageService, FixOrderMessageService>();
            services.AddScoped<IOrderAccumulatorApiService, OrderAccumulatorApiService>();

            services.AddHttpClient<IApiClient, ApiClient>(client =>
            {
                var options = configuration.GetSection("OrderAccumulatorApi").Get<OrderAccumulatorApiOptions>();
                client.BaseAddress = new Uri(options?.BaseUrl ?? throw new InvalidOperationException("AppSettings - OrderAccumulatorApi.BaseUrl não está configurado."));
                client.Timeout = TimeSpan.FromMinutes(options?.TimeoutMinutes ?? 60);
            });

            return services;
        }
    }
}
