using Microsoft.EntityFrameworkCore;

namespace RizSoft.Acme.Services;

public class TagService : BaseContextService<Tag, int>
{
    public TagService(IDbContextFactory<AcmeContext> factory) : base(factory)
    {
    }


   
}
