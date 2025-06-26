using OrdersProject.WebUI.Components;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Razor Components 
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();
// Notification service
builder.Services.AddScoped<NotificationService>();


// OrdersApi
builder.Services.AddHttpClient("OrdersAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiOptions:BaseUrl"]);
});

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>()
      .CreateClient("OrdersAPI"));

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStaticFiles();
app.UseAntiforgery();

// Https redirection only if not dev
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Blazor
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
