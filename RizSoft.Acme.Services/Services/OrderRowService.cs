
namespace RizSoft.Acme.Services;

public class OrderRowService : BaseContextService<OrderRow,int>
{
    public OrderRowService(IDbContextFactory<AcmeContext> factory) : base(factory)
    {

    }
}
