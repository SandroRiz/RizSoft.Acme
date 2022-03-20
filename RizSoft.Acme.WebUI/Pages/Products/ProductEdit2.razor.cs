using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace RizSoft.Acme.WebUI.Pages.Products;

public partial class ProductEdit2
{
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] NotificationService NotificationService { get; set; }
    //[Inject] ProductService ProductService { get; set; }
    //[Inject] TagService TagService { get; set; }
    [Inject] IDbContextFactory<AcmeContext> Factory { get; set; }

    public Product Product { get; set; }
    public List<Tag> Tags { get; set; }

    public List<int> ProductTags { get; set; } = new();
    public int SelectedTag { get; set; }

    private AcmeContext ctx;

    [Parameter]
    public int ProductId { get; set; }
    public string TagName { get; set; }
    protected async override Task OnInitializedAsync()
    {
        ctx = Factory.CreateDbContext();
        
            Tags = await ctx.Tags.ToListAsync();
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
        //Product = await ProductService.GetWithTagsAsync(ProductId);

        
            Product = await ctx.Products
            .Include(p => p.Tags)
            .Where(p => p.Id == ProductId)
            .FirstOrDefaultAsync();
        
    }
    protected async Task Save(Product arg)
    {
        if (SelectedTag > 0)
        {
            var tag = await ctx.Tags.FindAsync(SelectedTag);
            Product.Tags.Add(tag);
        }

         ctx.Products.Update(Product);
        await ctx.SaveChangesAsync();

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
