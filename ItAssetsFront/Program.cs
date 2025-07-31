using ItAssetsFront.Services.AuthService;
using ItAssetsFront.Services.BrandService;
using ItAssetsFront.Services.CategoryService;
using ItAssetsFront.Services.officeService;
using ItAssetsFront.Services.SupplierService;

namespace ItAssetsFront
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient<brandService>();
            builder.Services.AddHttpClient<categoryService>();
            builder.Services.AddHttpClient<LoginService>();
            builder.Services.AddHttpClient<officeService>();
            builder.Services.AddHttpClient<SupplierService>();
            builder.Services.AddSession();
        
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
