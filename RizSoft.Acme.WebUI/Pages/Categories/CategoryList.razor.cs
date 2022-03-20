using Radzen;
using Radzen.Blazor;

namespace RizSoft.Acme.WebUI.Pages.Categories;

public partial class CategoryList
{
    public List<Category> Categories { get; set; }

    RadzenDataGrid<Category> CategorysGrid;
    Category categoryToInsert;
    async Task LoadData()
    {
        Categories = await CategoryService.ListAsync();
    }

    async Task EditRow(Category category)
    {
        await CategorysGrid.EditRow(category);
    }

    async Task OnUpdateRow(Category category)
    {
        if (category == categoryToInsert)
        {
            categoryToInsert = null;
        }

        await CategoryService.UpdateAsync(category);
    }

    async Task SaveRow(Category category)
    {
        if (category == categoryToInsert)
        {
            categoryToInsert = null;
        }

        await CategorysGrid.UpdateRow(category);
    }

    void CancelEdit(Category category)
    {
        if (category == categoryToInsert)
        {
            categoryToInsert = null;
        }

        CategorysGrid.CancelEditRow(category);
    }

    async Task InsertRow()
    {
        categoryToInsert = new Category();
        await CategorysGrid.InsertRow(categoryToInsert);
    }

    async Task OnCreateRow(Category category)
    {
        await CategoryService.AddAsync(category);
    }

    async Task DeleteRow(Category category)
    {
        var confirm = await DialogService.Confirm("Are you sure?", "Delete Record", new ConfirmOptions()
        {OkButtonText = "Yes", CancelButtonText = "No"});
        if (confirm.HasValue && confirm.Value)
        {
            if (category == categoryToInsert)
            {
                categoryToInsert = null;
            }

            if (Categories.Contains(category))
            {
                await CategoryService.DeleteAsync(category);
                await CategorysGrid.Reload();
            }
            else
            {
                CategorysGrid.CancelEditRow(category);
            }
        }
    }
}
