namespace RizSoft.Acme.Services;

public class CategoryService : BaseService<Category,int>
{
    public CategoryService(IDbContextFactory<AcmeContext> factory) : base(factory)
    {
    }

   
}
