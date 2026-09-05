using OrderGenerator.Application.DTOs;
using OrderGenerator.Application.Interfaces;
using OrderGenerator.Domain.Entities;
using OrderGenerator.Domain.Enum;

namespace OrderGenerator.Application.Services
{
    public sealed class OrderService : IOrderService
    {
        private readonly ISymbolService _symbolService;
        private readonly IFixOrderMessageService _fixOrderMessageService;
        private readonly IOrderAccumulatorApiService _orderAccumulatorApiService;

        public OrderService(
            ISymbolService symbolService,
            IFixOrderMessageService fixOrderMessageService,
            IOrderAccumulatorApiService orderAccumulatorApiService)
        {
            _symbolService = symbolService;
            _fixOrderMessageService = fixOrderMessageService;
            _orderAccumulatorApiService = orderAccumulatorApiService;
        }

        public async Task<CreateOrderResponse> CreateOrderAsync(
            CreateOrderRequest request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Symbol))
            {
                throw new ArgumentException(
                    "Simbolo é obrigatório.");
            }

            var symbolExists =
                await _symbolService.IsValidAsync(
                    request.Symbol,
                    cancellationToken);

            if (!symbolExists)
            {
                throw new ArgumentException(
                    $"Simbolo '{request.Symbol}' não é válido.");
            }

            if (!Enum.TryParse<OrderSide>(
                    request.Side,
                    ignoreCase: true,
                    out var side))
            {
                throw new ArgumentException(
                    "Lado precisa ser BUY ou SELL.");
            }

            var order = new Order(
                request.Symbol,
                side,
                request.Amount,
                request.Price);

            var validationError =
                order.Validate();

            if (validationError is not null)
            {
                throw new ArgumentException(
                    validationError);
            }

            var fixMessage = await _fixOrderMessageService.CreateNewOrderSingle(order);

            //Apenas para facilitar o debug/testes 
            //var redableFixMessage = fixMessage.Replace('\u0001', '|');

            var response = await _orderAccumulatorApiService.SendOrderFixMessageAsync(fixMessage, cancellationToken);

            return new CreateOrderResponse
            {
                Message = "Ordem criada com sucesso!"
            };
        }
    }
}
