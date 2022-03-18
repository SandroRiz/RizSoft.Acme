

namespace RizSoft.Acme.Services;

public class CustomerService : BaseContextService<Customer,int>
{
    public CustomerService(IDbContextFactory<AcmeContext> factory) : base(factory)
    {

    }
}
