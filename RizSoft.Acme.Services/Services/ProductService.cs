﻿using Microsoft.EntityFrameworkCore;

namespace RizSoft.Acme.Services;

public class ProductService : BaseService<Product, int>
{
    public ProductService(IDbContextFactory<AcmeContext> factory) : base(factory)
    {
    }

    public async Task<Product> GetWithTagsAsync(int id)
    {
        //using var Context = ContextFactory.CreateDbContext();
        return await Context.Products
            .Include(p => p.IdTags)
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Product> AddTag(int idProduct, Tag tag)
    {
        //using var Context = ContextFactory.CreateDbContext();
        var product = await Context.Products.FindAsync(idProduct);
        if (product != null)
        {
            product.IdTags.Add(tag);
            await Context.SaveChangesAsync();
            return product;
        }
        else
            return null;
    }

    public async Task<List<Product>> ListActive()
    {
        //using var Context = ContextFactory.CreateDbContext();
        return await Context.Products
            .Include(p => p.Category)
            .Where(e => !e.Discontinued)
            .OrderBy(e => e.ProductName)
            .ToListAsync();

    }


}
