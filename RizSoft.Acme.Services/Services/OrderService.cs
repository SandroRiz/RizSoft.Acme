
namespace RizSoft.Acme.Services;

public class OrderService : BaseService<Order, int>
{
    public OrderService(IDbContextFactory<AcmeContext> factory) : base(factory)
    { }
    public override async Task<List<Order>> ListAsync()
    {
        //using var ctx = CtxFactory.CreateDbContext();
        return await Context.Orders
            .Include(o => o.OrderRows)
            .Include( o => o.Customer)
            .ToListAsync();
    }

    public override async Task<Order> GetAsync(int id)
    {
        //using var ctx = CtxFactory.CreateDbContext();
        return await Context.Orders
            .Where(o => o.Id == id)
            .Include(o => o.OrderRows)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync();
    }


}
