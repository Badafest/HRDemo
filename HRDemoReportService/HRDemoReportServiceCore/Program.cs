using HRDemoCoreReportService = HRDemoReportServiceCore.HRDemoReportService;

var builder = WebApplication.CreateBuilder();

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();

builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();
builder.Services.AddSingleton<ServiceAuthorizationManager, ApiKeyAuthorizationManager>();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<HRDemoCoreReportService>();

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<HRDemoCoreReportService>(options =>
    {
        options.DebugBehavior.IncludeExceptionDetailInFaults = builder.Environment.IsEnvironment("Local");
    });

    List<BindingInfo> bindingInfo = [new()];

    if (builder.Configuration["UseHttps"] == "true")
    {
        bindingInfo.Add(new() { Binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport) });
    }

    foreach (var binding in bindingInfo)
    {
        serviceBuilder.AddServiceEndpoint<HRDemoCoreReportService, IHRDemoReportService>(binding.Binding, binding.Endpoint);
    }
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();

    serviceMetadataBehavior.HttpsGetEnabled = true;
    serviceMetadataBehavior.HttpGetEnabled = true;
});

app.Run();