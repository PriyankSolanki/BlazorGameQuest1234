using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorGame.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(sp => new HttpClient 
{
    BaseAddress = new Uri("http://localhost:5002/") 
});


builder.Services.AddScoped<GameApiService>();

await builder.Build().RunAsync();
