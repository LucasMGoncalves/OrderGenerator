using Microsoft.AspNetCore.Http;
using OrderGenerator.Application.Interfaces;
using System.Text;

namespace OrderGenerator.Infrastructure.Integrations
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiClient(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> PostAsync(
            string path,
            string content,
            string contentType,
            CancellationToken cancellationToken)
        {

            using var request = new HttpRequestMessage(HttpMethod.Post, path);
            
            request.Content = new StringContent(
                content,
                Encoding.UTF8,
                contentType);

            var authorization = _httpContextAccessor
                .HttpContext?
                .Request
                .Headers
                .Authorization
                .ToString();

            if (!string.IsNullOrWhiteSpace(authorization))
            {
                request.Headers.TryAddWithoutValidation(
                    "Authorization",
                    authorization);
            }

            using var response = await _httpClient.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            return responseContent
                ?? throw new InvalidOperationException("A API não retornou conteúdo.");
        }
    }
}
