using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace RizSoft.Acme.WebUI.Pages.Orders;

public partial class OrderEdit2
{
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] NotificationService NotificationService { get; set; }
    //[Inject] OrderService OrderService { get; set; }
    //[Inject] TagService TagService { get; set; }

    [Inject] IDbContextFactory<AcmeContext> Factory { get; set; }

    public Order Order { get; set; }
    public List<Tag> Tags { get; set; }

    public List<int> OrderTags { get; set; } = new();
    public int SelectedTag { get; set; }

    private AcmeContext ctx;

    [Parameter]
    public int OrderId { get; set; }
    public string TagName { get; set; }

    protected async override Task OnInitializedAsync()
    {
        ctx = Factory.CreateDbContext();
        await LoadAsync();
        await base.OnInitializedAsync();
    }

    protected async override Task OnParametersSetAsync()
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
      
        Order =  await ctx.Orders
            .Where(o => o.Id == OrderId)
            .Include(o => o.OrderRows)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync();



    }

    protected async Task OnChange(dynamic value)
    {
        await LoadAsync();
    }

    protected void UpdateDate()
    {
        Order.OrderDate = DateTime.Now;
        Order.Customer.Email = $"test_{DateTime.Now.Second}@mydomain.com";



    }

    protected void AddRow()
    {
        int lastRow = Order.OrderRows.Count;
        lastRow++;
        OrderRow row = new OrderRow { RowNumber = lastRow, ProductId = lastRow, Qty = lastRow * 10, UnitPrice = lastRow * 100, Discount = 0 };
        Order.OrderRows.Add(row);
    }

    protected void DeleteRow()
    {
        OrderRow row = Order.OrderRows.FirstOrDefault();
        Order.OrderRows.Remove(row);
    }


    protected async Task Save()
    {
        ctx.Orders.Update(Order);
        await ctx.SaveChangesAsync();
        

        NotificationService.Notify(NotificationSeverity.Success, "Order Saved succefully");

    }
}