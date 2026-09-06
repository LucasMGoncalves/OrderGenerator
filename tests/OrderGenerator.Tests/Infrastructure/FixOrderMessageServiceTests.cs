using OrderGenerator.Domain.Entities;
using OrderGenerator.Domain.Enum;
using OrderGenerator.Infrastructure.Services;
using Xunit;

namespace OrderGenerator.Tests.Infrastructure
{
    public class FixOrderMessageServiceTests
    {
        [Fact]
        public async Task CreateNewOrderSingle_MessageShouldBeNotNull_WhenOrderIsValid()
        {
            var service = new FixOrderMessageService();

            var order = new Order(
                "PETR4",
                OrderSide.BUY,
                2,
                100m);

            var fixMessage = await service.CreateNewOrderSingle(order);

            //TODO: [TEC] Melhorar: Efetivamente verificar se a mensagem FIX gerada é válida de acordo com o protocolo FIX.
            //Por hora a validação foi realizada manualmente utilizando um serviço terceiro (fixmsg.com/tools/validator)
            Assert.True(!string.IsNullOrWhiteSpace(fixMessage));
        }

        [Fact]
        public async Task CreateNewOrderSingle_ShouldThrowArgumentOutOfRangeException_WhenSideIsInvalid()
        {
            var service = new FixOrderMessageService();

            var order = new Order(
                "PETR4",
                (OrderSide)999,
                2,
                100m);

            var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => service.CreateNewOrderSingle(order));

            Assert.Equal("side", exception.ParamName);
            Assert.Contains("Lado não suportado.", exception.Message);
        }
    }
}
