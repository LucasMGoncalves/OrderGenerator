namespace OrderGenerator.Application.Interfaces
{
    public interface IApiClient
    {
        Task<TResponse> PostAsync<TResponse>(
            string path,
            string content,
            string contentType,
            CancellationToken cancellationToken);
    }
}
