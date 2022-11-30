using Microsoft.EntityFrameworkCore;
using RizSoft.Acme.Domain.Models;

namespace RizSoft.Acme.Services;

public class TagService : BaseService<Tag, int>
{
    public TagService(IDbContextFactory<AcmeContext> factory) : base(factory)
    {
    }


   public async Task<List<Tag>> GetTagsByProductAsync(int productId)
    {
        using var ctx = CtxFactory.CreateDbContext();
        return await ctx.Tags
            .Where(p => p.Products.Any(p => p.Id == productId) )
            .OrderBy(t => t.Id)
            .ToListAsync();
    }
}
