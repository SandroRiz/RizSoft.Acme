namespace RizSoft.Acme.WebUI.Services
{
    public static class ServiceConfiguration
    {
        public static void AddAcmeServices(this IServiceCollection services)
        {
            services.AddTransient<ProductService>();
            services.AddTransient<CategoryService>();
        }
    }
}
