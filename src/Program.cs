using LiteDB;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using LiteDB.Engine;
using Microsoft.JSInterop;
using Restaurant.Template.Utility;

namespace Restaurant.Template
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddMudServices();
            
            //Register Timer to perform every 5 minutes.
            builder.Services.AddSingleton<PeriodicExecutor>();
            
            //Uses LiteDB.WebAssembly for local storage and sync post offline storage.
            builder.Services.AddTransient<ILiteDatabase>(sp => new LiteDatabase(new LocalStorageStream(sp.GetService<IJSRuntime>())));
            await builder.Build().RunAsync();
        }
    }
}
