using OrderGenerator.Application.Interfaces;
using OrderGenerator.Domain.Entities;
using OrderGenerator.Domain.Enum;
using QuickFix;
using QuickFix.Fields;
using QuickFix.FIX44;

namespace OrderGenerator.Infrastructure.Services
{
    public class FixOrderMessageService
    : IFixOrderMessageService
    {
        public Task<string> CreateNewOrderSingle(Order order)
        {
            var fixOrder = new NewOrderSingle(
                new ClOrdID(CreateClientOrderId()),
                new QuickFix.Fields.Symbol(order.Symbol),
                MapSide(order.Side),
                new TransactTime(DateTime.UtcNow),
                new OrdType(OrdType.LIMIT)
            );

            fixOrder.Set(new QuickFix.Fields.Symbol(order.Symbol));
            fixOrder.Set(new OrderQty(order.Amount));
            fixOrder.Set(new Price(order.Price));
            
            return Task.Run(() => fixOrder.ConstructString());
        }

        private static Side MapSide(OrderSide side)
        {
            return side switch
            {
                OrderSide.BUY => new Side(Side.BUY),

                OrderSide.SELL => new Side(Side.SELL),

                _ => throw new ArgumentOutOfRangeException(
                    nameof(side),
                    side,
                    "LUnsupported order side.")
            };
        }

        private static string CreateClientOrderId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
