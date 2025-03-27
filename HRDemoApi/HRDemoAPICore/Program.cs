using HRDemoAPI.DataCore.Models;
using HRDemoAPICore.Filters;
using HRDemoAPICore.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using static HRDemoAPICore.Filters.NullToEmptyStringModelBinder;

var builder = WebApplication.CreateBuilder(args);
HttpUtilities.Configuration = builder.Configuration;

// these are no longer required as the migration is completed
// builder.Services.AddSystemWebAdapters();
// builder.Services.AddHttpForwarder();

// Add services to the container.
builder.Services.AddDbContext<HRDemoApiContext>(options => {
    options.LogTo(Console.WriteLine);
    options.UseSqlServer(builder.Configuration.GetConnectionString("HRDemoApiDb"));
});

builder.Services.AddScoped<HRDemoAuthorizeFilter>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration);

// Configure model state validation filter, null string binder and string enum converter
builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ValidateModelAttribute>(); 
        options.ModelBinderProviders.Insert(0, new NullToEmptyStringModelBinderProvider());
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
        // Prevent Newtonsoftjson from serializing to camel case
        options.SerializerSettings.ContractResolver = new DefaultContractResolver()
        {
            NamingStrategy = null
        };
    })
    // This is to generate pascal cased schema in swagger UI
    .AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // This auth flow is pretty similar to what the migrated MVC application will follow
    // except instead of relying on PKCE, it generates the token using client secret
    // and then pass the generated token in request headers.

    // Client ID and PKCE are set later in application middleware app.UseSwaggerUI

    // Define the security scheme
    options.AddSecurityDefinition("oidc", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(builder.Configuration["AzureAd:Instance"] + builder.Configuration["AzureAd:TenantId"] + "/oauth2/v2.0/authorize"),
                TokenUrl = new Uri(builder.Configuration["AzureAd:Instance"] + builder.Configuration["AzureAd:TenantId"] + "/oauth2/v2.0/token"),
                Scopes = new Dictionary<string, string>
                {
                    { $"{builder.Configuration["AzureAd:ClientId"]}/.default", "Sign you in and access your profile" },
                }
            }
        },
    });

    // Apply Security Requirement
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oidc"
                }
            },
            []
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.OAuthClientId(builder.Configuration["AzureAd:ClientId"]);
    options.OAuthUsePkce(); // Use PKCE for security
});

// app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

// app.UseSystemWebAdapters();

app.MapControllers();
// app.MapForwarder("/{**catch-all}", app.Configuration["ProxyTo"] ?? "").Add(static builder => ((RouteEndpointBuilder)builder).Order = int.MaxValue);

app.Run();
