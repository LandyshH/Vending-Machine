namespace Domain.Orders;

public interface IOrderRepository
{
    Task<Order> GetByIdAsync(int id);

    Task<ICollection<Order>> GetAllAsync();

    Task AddAsync(Order order);

    Task SaveAsync(Order order);
}
