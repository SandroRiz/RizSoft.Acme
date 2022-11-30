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
    public IEnumerable<int> SelectedTagIds { get; set; } 


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
        // Product con collection di tags
        //Product = await ProductService.GetWithTagsAsync(ProductId);
        //SelectedTagIds = Product.Tags.Select(x => x.Id).ToList();

        //oppure tiro su il product senza i tag
        Product = await ProductService.GetAsync(ProductId);
        var selectedTags = await TagService.GetTagsByProductAsync(ProductId);
        SelectedTagIds = selectedTags.Select( x => x.Id).ToList();

    }

    protected async Task Save(Product arg)
    {
       
        try
        {
            // salva le prop di Product; 
            await ProductService.UpdateAsync(Product);

            // se non si riesce a fare tutto in un colpo solo, metodo per salvare solo i tag
            await ProductService.SaveTags(Product.Id, SelectedTagIds);
       
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

    protected void Cancel()
    {
        NavigationManager.NavigateTo("/products");
    }

    void TagsChanged(object value)
    {
        
    }
}
