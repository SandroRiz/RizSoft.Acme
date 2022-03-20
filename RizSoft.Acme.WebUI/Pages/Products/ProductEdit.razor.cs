using Microsoft.AspNetCore.Components;
using Radzen;

namespace RizSoft.Acme.WebUI.Pages.Products;

public partial class ProductEdit
{
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] NotificationService NotificationService { get; set; }
    [Inject] ProductService ProductService { get; set; }
    [Inject] TagService TagService { get; set; }

    public Product Product { get; set; }
    public List<Tag> Tags { get; set; }

    public List<int> ProductTags { get; set; } = new();
    public int SelectedTag { get; set; } 


    [Parameter]
    public int ProductId { get; set; }
    public string TagName { get; set; }
    protected async override Task OnInitializedAsync()
    {
        Tags = await TagService.ListAsync();
        await LoadAsync();
        await base.OnInitializedAsync();
    }

    protected async override Task OnParametersSetAsync()
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        //Product = await ProductService.GetAsync(ProductId);
        Product = await ProductService.GetWithTagsAsync(ProductId);
        
        foreach (var item in Product.Tags)
        {
            ProductTags.Add(item.Id);
        }

    }

    protected async Task Save(Product arg)
    {
        if (SelectedTag > 0)
            Product.Tags.Add(await TagService.GetAsync(SelectedTag));
       
        await ProductService.UpdateAsync(Product);
       
        NotificationService.Notify(NotificationSeverity.Success, "Product Saved succefully");
       
    }

    protected async Task Cancel()
    {
        NavigationManager.NavigateTo("/product/list");
    }

    void TagsChanged(object value)
    {
        
    }
}
