namespace RizSoft.Acme.Services;

public class BaseService<T, TKey> :  IBaseRepository<T, TKey>
where T : class
{
    protected IDbContextFactory<AcmeContext> CtxFactory { get; }
    public BaseService(IDbContextFactory<AcmeContext> ctxFactory)
    {
        CtxFactory = ctxFactory;
    }

    public virtual async Task<T> GetAsync(TKey id)
    {
        await using var ctx = await CtxFactory.CreateDbContextAsync();
        var set = ctx.Set<T>();
        if (set == null) throw new ArgumentException(nameof(set));

        var entity = await set.FindAsync(id);
        if (entity == null) throw new ArgumentException($"Cannot find id {id}");

        return entity;
    }

    public virtual async Task<List<T>> ListAsync()
    {
        await using var ctx = await CtxFactory.CreateDbContextAsync();
        var set = ctx.Set<T>();
        return await set.ToListAsync();
    }


    public virtual async Task AddAsync(T entity)
    {
        await using var ctx = await CtxFactory.CreateDbContextAsync();
        var set = ctx.Set<T>();
        set.Add(entity);
        await ctx.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        await using var ctx = await CtxFactory.CreateDbContextAsync();
        ctx.Set<T>().Update(entity);   
        //non salvava le related entities
        //ctx.Entry(entity).State = EntityState.Modified;
        await ctx.SaveChangesAsync();
    }



    public virtual async Task DeleteAsync(T entity)
    {
        await using var ctx = await CtxFactory.CreateDbContextAsync();

       
        ctx.Set<T>().Remove(entity);

        //ctx.Entry(entity).State = EntityState.Deleted;
        await ctx.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TKey id)
    {
        await using var ctx = await CtxFactory.CreateDbContextAsync();
        var set = ctx.Set<T>();

        T? entity = await set.FindAsync(id);
        if (entity == null) throw new ArgumentException($"Cannot find id {id}");

        //why not
        set.Remove(entity);

        //ctx.Entry(entity).State = EntityState.Deleted;
        await ctx.SaveChangesAsync();
    }





}