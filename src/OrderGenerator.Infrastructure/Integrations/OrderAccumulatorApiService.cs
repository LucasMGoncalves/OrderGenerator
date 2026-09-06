using Microsoft.Extensions.Options;
using OrderGenerator.Application.DTOs;
using OrderGenerator.Application.Interfaces;
using OrderGenerator.Infrastructure.Configurations;
using QuickFix.DataDictionary;
using QuickFix.Fields;

namespace OrderGenerator.Infrastructure.Integrations
{
    public class OrderAccumulatorApiService : IOrderAccumulatorApiService
    {
        private readonly IApiClient _apiClient;
        private readonly OrderAccumulatorApiOptions _options;
        private readonly DataDictionary _dataDictionary;

        public OrderAccumulatorApiService(
            IApiClient apiClient,
            IOptions<OrderAccumulatorApiOptions> options)
        {
            _apiClient = apiClient;
            _options = options.Value;

            var dictionaryPath = Path.Combine($"{AppContext.BaseDirectory}/Resources", "FIX44.xml");

            if (!File.Exists(dictionaryPath))
            {
                throw new FileNotFoundException(
                    "O arquivo FIX44.xml não foi encontrado.",
                    dictionaryPath);
            }

            _dataDictionary = new DataDictionary(dictionaryPath);
        }

        public async Task<ExecutionReportResponse> SendOrderFixMessageAsync(
            string fixMessage,
            CancellationToken cancellationToken)
        {
            var responseFixMessage = await _apiClient.PostAsync(
                _options.ReceiveOrderPath,
                fixMessage,
                "text/plain",
                cancellationToken);

            return ParseExecutionReport(responseFixMessage);
        }

        private ExecutionReportResponse ParseExecutionReport(string fixMessage)
        {
            var message = new QuickFix.Message();

            message.FromString(
                fixMessage,
                true,
                _dataDictionary,
                _dataDictionary,
                null);

            var execType = message.GetChar(Tags.ExecType);

            return new ExecutionReportResponse
            {
                ClientOrderId = message.GetString(Tags.ClOrdID),

                Accepted = execType == ExecType.NEW,

                Text = message.IsSetField(Tags.Text) ? message.GetString(Tags.Text) : null
            };
        }
    }
}
