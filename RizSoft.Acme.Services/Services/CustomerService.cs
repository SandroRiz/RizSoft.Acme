

namespace RizSoft.Acme.Services;

public class CustomerService : BaseService<Customer,int>
{
    public CustomerService(IDbContextFactory<AcmeContext> factory) : base(factory)
    {

    }
}
