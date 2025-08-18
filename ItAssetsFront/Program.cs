using ItAssetsFront.Services.AssignService;
using ItAssetsFront.Services.AuthService;
using ItAssetsFront.Services.BrandService;
using ItAssetsFront.Services.CategoryService;
using ItAssetsFront.Services.DeviceMaintainanceService;
using ItAssetsFront.Services.DeviceRequestService;
using ItAssetsFront.Services.DeviceService;
using ItAssetsFront.Services.EmployeeService;
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
            builder.Services.AddHttpClient<EmployeeService>();
            builder.Services.AddHttpClient<DeviceService>();
            builder.Services.AddHttpClient<AssignService>();
            builder.Services.AddHttpClient<OfficeAssignService>();
            builder.Services.AddHttpClient<RequestService>(); 
            builder.Services.AddHttpClient<MaintainanceRequest>();
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
                pattern: "{controller=Login}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
