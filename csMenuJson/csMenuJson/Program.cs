using csMenuJson.Components;
using csMenuJson.Services;
using Syncfusion.Blazor;

namespace csMenuJson
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddTransient<MenuDataRoleBuildService>();
            builder.Services.AddSyncfusionBlazor();

            var app = builder.Build();

            var syncfusionLicense = app.Configuration.GetValue<string>("SyncfusionLicense");
            if (!string.IsNullOrEmpty(syncfusionLicense))
            {
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicense);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
