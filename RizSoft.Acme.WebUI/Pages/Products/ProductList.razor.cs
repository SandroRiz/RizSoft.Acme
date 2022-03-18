using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System;

namespace RizSoft.Acme.WebUI.Pages.Products
{
    public partial class ProductList
    {
        public List<Product>? Products { get; set; }

        public List<Category>? Categories { get; set; }

        [Parameter]
        public int? CategoryId { get; set; }

        string currentCostingMethod = "";
        Dictionary<string, string> CostingMethods = new Dictionary<string, string>()
        {{"-", "(All)"}, {"L", "LIFO"}, {"F", "FIFO"}};
        IList<Product>? currentProduct;
        ProductTypeEnum? currentProductType;
        public enum ProductTypeEnum : byte
        {
            Product = 1,
            Service = 2,
            Digital = 3,
            All = 0
        }

        private int? currentCategory;
        public string TagName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Products = await ProductService.ListActiveAsync();
            Categories = await CategoryService.ListAsync();
            Categories.Insert(0, new Category{Id = 0, Name = "(All)"});
        }

        protected override async Task OnParametersSetAsync()
        {
            if (CategoryId.HasValue)
                Products = await ProductService.ListbyCategoryAsync(CategoryId.Value);
        }

        void OnFilterCostingMethodChange(object value)
        {
            if (currentCostingMethod == "-")
                currentCostingMethod = null;
        }

        void OnFilterCategoryChange(object value)
        {
            if (currentCategory.HasValue && currentCategory.Value == 0)
                currentCategory = null;
        }

        void CellRender(DataGridCellRenderEventArgs<Product> args)
        {
            if (args.Column.Property == "ListPrice")
            {
                args.Attributes.Add("style", $"background-color: {(args.Data.ListPrice > (decimal)700 ? "red" : "green")};");
            }
        }

        void RowRender(RowRenderEventArgs<Product> args)
        {
            args.Attributes.Add("style", $"font-weight: {(args.Data.PackDimension > 100 ? "bold" : "normal")};");
        }

        protected async Task SaveTag()
{
            Product p = currentProduct?[0];
            if (p != null)
            {
                Tag tag = new Tag();
                tag.TagName = TagName;
                await TagService.AddAsync(tag);
                p.Tags.Add(tag);
               
                await ProductService.UpdateAsync(p);  
            }
        }
    }
}