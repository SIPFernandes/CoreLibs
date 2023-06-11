using BlazorCore.Areas.Interfaces;
using BlazorCore.Areas.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorCore
{
    public class Dependencies : ComponentBase
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IJSInteropService, JSInteropService>();
            services.AddScoped<IDialogService, DialogService>();            
        }
    }
}
