
namespace RizSoft.Acme.Services;

public class OrderRowService : BaseService<OrderRow,int>
{
    public OrderRowService(IDbContextFactory<AcmeContext> factory) : base(factory)
    {

    }
}
