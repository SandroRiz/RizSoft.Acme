namespace RizSoft.Acme.Services;

public class CategoryService : BaseContextService<Category,int>
{
    public CategoryService(IDbContextFactory<AcmeContext> factory) : base(factory)
    {
    }

   
}
