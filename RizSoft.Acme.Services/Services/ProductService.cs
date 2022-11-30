using Microsoft.EntityFrameworkCore;
using RizSoft.Acme.Domain.Models;

namespace RizSoft.Acme.Services;

public class ProductService : BaseService<Product, int>
{
    public ProductService(IDbContextFactory<AcmeContext> factory) : base(factory)
    {
        
    }

    public override async Task<Product?> GetAsync(int id)
    {
        using var ctx = CtxFactory.CreateDbContext();
        return await ctx.Products
            .Where(p => p.Id == id)
            .AsNoTracking()     // se nell'update sync se ne tira su un altro per updatarlo serve altrimenti ne trackerebbe 2
            .FirstOrDefaultAsync();
    }

    public async Task<Product?> GetWithTagsAsync(int id)
    {
        using var ctx = CtxFactory.CreateDbContext();
        return await ctx.Products
            .Include(p => p.Tags)
            .Where(p => p.Id == id)
            .AsNoTracking()     // se nell'update sync se ne tira su un altro per updatarlo serve altrimenti ne trackerebbe 2
            .FirstOrDefaultAsync();
    }

    public override async Task UpdateAsync(Product product)
    {
        using var ctx = await CtxFactory.CreateDbContextAsync();

        //LE HO PROVATE TUTTE!!

        ctx.Products.Update(product);
        await ctx.SaveChangesAsync();
    }

    public async Task SaveTags(int id, IEnumerable<int> selectedTagIds)
    {
        using var ctx = await CtxFactory.CreateDbContextAsync();

        Product p = await GetWithTagsAsync(id);
        List<Tag> tags = new List<Tag>();
        foreach (int tagId in selectedTagIds)
        {
            tags.Add(await ctx.Tags.FindAsync(tagId));
        }
        p.Tags = tags;

        ctx.Products.Update(p);
        await ctx.SaveChangesAsync();
    }


    public async Task<List<Product>> ListActiveAsync()
    {
       using var ctx = CtxFactory.CreateDbContext();
        return await ctx.Products
            .Include(p => p.Category)
            .Where(e => !e.Discontinued.Value)
            .OrderBy(e => e.ProductName)
            .ToListAsync();
    }

    public async Task<List<Product>> ListbyCategoryAsync(int categoryId)
    {
       using var ctx = CtxFactory.CreateDbContext();
        return await ctx.Products
            .Include(p => p.Category)
            .Where(p=> p.CategoryId == categoryId)
            .OrderBy(e => e.ProductName)
            .ToListAsync();

    }


}
