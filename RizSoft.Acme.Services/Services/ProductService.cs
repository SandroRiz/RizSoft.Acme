using Microsoft.EntityFrameworkCore;

namespace RizSoft.Acme.Services;

public class ProductService : BaseService<Product, int>
{
    public ProductService(IDbContextFactory<AcmeContext> factory) : base(factory)
    {
    }

    public async Task<List<Product>> ListActive()
    {
        using var ctx = CtxFactory.CreateDbContext();
        return await ctx.Products
            .Include( p => p.Category)
            .Where(e => !e.Discontinued)
            .OrderBy(e => e.ProductName)
            .ToListAsync();

    }

   
}
