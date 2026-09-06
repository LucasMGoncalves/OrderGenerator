namespace OrderGenerator.Application.Interfaces
{
    public interface IApiClient
    {
        Task<string> PostAsync(
            string path,
            string content,
            string contentType,
            CancellationToken cancellationToken);
    }
}
