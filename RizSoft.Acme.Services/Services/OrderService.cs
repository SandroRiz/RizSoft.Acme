
namespace RizSoft.Acme.Services;

public class OrderService : BaseService<Order, int>
{
    public OrderService(IDbContextFactory<AcmeContext> factory) : base(factory)
    { }
    public override async Task<List<Order>> ListAsync()
    {
        using var ctx = CtxFactory.CreateDbContext();
        return await ctx.Orders
            .Include(o => o.OrderRows)
            .Include( o => o.Customer)
            .ToListAsync();
    }

    public async Task<Order?> GetAsyncWithRows(int id)
    {
        using var ctx = CtxFactory.CreateDbContext();
        return await ctx.Orders
            .Where(o => o.Id == id)
            .Include(o => o.OrderRows)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync();
    }

    

}
