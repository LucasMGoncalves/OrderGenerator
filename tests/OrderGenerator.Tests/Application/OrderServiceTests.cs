using Moq;
using OrderGenerator.Application.DTOs;
using OrderGenerator.Application.Interfaces;
using OrderGenerator.Application.Services;
using OrderGenerator.Domain.Entities;
using OrderGenerator.Domain.Enum;
using Xunit;

namespace OrderGenerator.Tests.Application
{
    public sealed class OrderServiceTests
    {
        private readonly Mock<ISymbolService> _symbolServiceMock;
        private readonly Mock<IFixOrderMessageService> _fixOrderMessageServiceMock;
        private readonly Mock<IOrderAccumulatorApiService> _orderAccumulatorApiServiceMock;

        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _symbolServiceMock = new Mock<ISymbolService>();

            _fixOrderMessageServiceMock = new Mock<IFixOrderMessageService>();

            _orderAccumulatorApiServiceMock = new Mock<IOrderAccumulatorApiService>();

            _service = new OrderService(
                _symbolServiceMock.Object,
                _fixOrderMessageServiceMock.Object,
                _orderAccumulatorApiServiceMock.Object);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldReturnSuccess_WhenOrderIsValid()
        {
            var orderTest = new Order(
                "PETR4",
                OrderSide.BUY,
                2,
                100m);

            var fixMessage = "FIX-MESSAGE-TEST";

            var request = new CreateOrderRequest
            {
                Symbol = orderTest.Symbol,
                Side = orderTest.Side.ToString().ToUpper(),
                Amount = orderTest.Amount,
                Price = orderTest.Price
            };

            _symbolServiceMock.Setup(service => service.IsValidAsync(
                orderTest.Symbol,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _fixOrderMessageServiceMock.Setup(service => service.CreateNewOrderSingle(
                It.IsAny<Order>()))
                .ReturnsAsync(fixMessage);

            _orderAccumulatorApiServiceMock.Setup(service => service.SendOrderFixMessageAsync(
                fixMessage,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ExecutionReportResponse
                {
                    Accepted = true
                });

            var result = await _service.CreateOrderAsync(request, CancellationToken.None);

            Assert.NotNull(result);

            Assert.Equal("Ordem criada com sucesso!", result.Message);

            _symbolServiceMock.Verify(service => service.IsValidAsync(
                orderTest.Symbol,
                It.IsAny<CancellationToken>()),
                Times.Once);

            _fixOrderMessageServiceMock.Verify(service => service.CreateNewOrderSingle(
                It.Is<Order>(order =>
                order.Symbol == orderTest.Symbol &&
                order.Side == orderTest.Side &&
                order.Amount == orderTest.Amount &&
                order.Price == orderTest.Price)),
                Times.Once);

            _orderAccumulatorApiServiceMock.Verify(service => service.SendOrderFixMessageAsync(
                fixMessage,
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
