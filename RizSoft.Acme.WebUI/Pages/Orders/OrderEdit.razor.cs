using Microsoft.AspNetCore.Components;
using Radzen;
using System;

namespace RizSoft.Acme.WebUI.Pages.Orders;

public partial class OrderEdit
{
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] NotificationService NotificationService { get; set; }
    [Inject] OrderService OrderService { get; set; }
    [Inject] TagService TagService { get; set; }

    public Order Order { get; set; }
    public List<Tag> Tags { get; set; }

    public List<int> OrderTags { get; set; } = new();
    public int SelectedTag { get; set; }


    [Parameter]
    public int OrderId { get; set; }
    public string TagName { get; set; }
    protected async override Task OnInitializedAsync()
    {

        await LoadAsync();
        await base.OnInitializedAsync();
    }

    protected async override Task OnParametersSetAsync()
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        //Order = await OrderService.GetAsync(OrderId);
        Order = await OrderService.GetAsyncWithRows(OrderId);



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
        OrderRow row = new OrderRow { RowNumber = lastRow, ProductId = lastRow, Qty = lastRow * 10, UnitPrice = lastRow * 100, Discount=0 };
        Order.OrderRows.Add(row);
    }

    protected void DeleteRow()
    {
        OrderRow row = Order.OrderRows.FirstOrDefault();
        Order.OrderRows.Remove(row);
    }


    protected async Task Save()
    {
     
        await OrderService.UpdateAsync(Order);

        NotificationService.Notify(NotificationSeverity.Success, "Order Saved succefully");

    }
}