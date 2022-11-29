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
    public IEnumerable<int> SelectedTags { get; set; } 


    [Parameter]
    public int ProductId { get; set; }
    public string TagName { get; set; }
    protected async override Task OnInitializedAsync()
    {
        Tags = await TagService.ListAsync();

    }

    protected async override Task OnParametersSetAsync()
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        
        Product = await ProductService.GetWithTagsAsync(ProductId);
        SelectedTags = Product.Tags.Select(x => x.Id).ToList();
     
    }

    protected async Task Save(Product arg)
    {
       
        try
        {
            await ProductService.UpdateAsync(Product);
       
            NotificationService.Notify(NotificationSeverity.Success, "Product Saved succefully");
        }
        catch (Exception ex)
        {
            var msg = ex.Message;
            if (ex.InnerException != null)
                msg += ex.InnerException.Message;

            NotificationService.Notify(NotificationSeverity.Error, "ERROR",msg,10000);
        }
        
       
    }

    protected async Task Cancel()
    {
        NavigationManager.NavigateTo("/products");
    }

    void TagsChanged(object value)
    {
        
    }
}
