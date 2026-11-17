using less2_task.Models;
using Microsoft.EntityFrameworkCore;

namespace less2_task.Services;

public class OrderService(ApiDbContext dbContext)
{
    public async Task<ICollection<Order>> GetAllOrdersAsync()
    {
        return await dbContext.Orders.ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> AddOrderAsync(Order newOrder)
    {
        newOrder.OrderDate = DateTime.Now;
        newOrder.IsCompleted = false;

        await dbContext.Orders.AddAsync(newOrder);
        await dbContext.SaveChangesAsync();
        return newOrder;
    }

    public async Task<Order?> UpdateOrderQuantityAsync(int id, int newQuantity)
    {
        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if(order == null)
        {
            return null;
        }
        order.Quantity = newQuantity;
        await dbContext.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> UpdateOrderStatusAsync(int id, bool isCompleted)
    {
        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
        {
            return null;
        }
        order.IsCompleted = isCompleted;
        await dbContext.SaveChangesAsync();
        return order;
    }
}
