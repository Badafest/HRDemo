using HRDemoReportUI.ServiceCore;
using HRDemoReportUICore;
using HRDemoReportUICore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton((services) =>
{
    var configuration = builder.Configuration;
    return new ReportService(configuration["ServiceApiUrl"], configuration["ServiceApiKey"]);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserService>();

// OIDC Authentication

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
