namespace RizSoft.Acme.Services;

public class BaseContextService<T, Tkey> 
where T : class
{

    protected AcmeContext Context { get; }
    protected IDbContextFactory<AcmeContext> CtxFactory { get; }

    public BaseContextService(AcmeContext context)
    {
        this.Context = context;
    }

    public BaseContextService(IDbContextFactory<AcmeContext> factory)
    {
        this.Context = factory.CreateDbContext();
        this.CtxFactory = factory;
    }
    public DbSet<T> Set => Context.Set<T>();

    public virtual async Task AddAsync(T entity)
    {
        Set.Add(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        Context.Entry(entity).State = EntityState.Deleted;
        await Context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Tkey id)
    {
        T entity = await Set.FindAsync(id);

        Context.Entry(entity).State = EntityState.Deleted;
        await Context.SaveChangesAsync();
    }

    public virtual async Task<T> GetAsync(Tkey id)
    {
        return await Set.FindAsync(id);
    }

    public virtual async Task<List<T>> ListAsync()
    {
        return await Set.ToListAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        Context.Entry(entity).State = EntityState.Modified;

        await Context.SaveChangesAsync();
    }


}
