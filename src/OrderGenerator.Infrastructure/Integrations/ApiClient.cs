using OrderGenerator.Application.Interfaces;
using System.Net.Http.Json;
using System.Text;

namespace OrderGenerator.Infrastructure.Integrations
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse> PostAsync<TResponse>(
            string path,
            string content,
            string contentType,
            CancellationToken cancellationToken)
        {
            using var requestContent = new StringContent(
                content,
                Encoding.UTF8,
                contentType);

            using var response = await _httpClient.PostAsync(
                path,
                requestContent,
                cancellationToken);

            response.EnsureSuccessStatusCode();//TODO: [TEC] Verificar

            var responseContent = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
            
            return responseContent
                ?? throw new InvalidOperationException("A API não retornou conteúdo.");
        }
    }
}
