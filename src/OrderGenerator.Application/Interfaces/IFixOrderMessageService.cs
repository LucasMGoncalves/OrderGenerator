using OrderGenerator.Domain.Entities;

namespace OrderGenerator.Application.Interfaces
{
    public interface IFixOrderMessageService
    {
        Task<string> CreateNewOrderSingle(Order order);
    }
}
