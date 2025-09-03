using Amazon.SecretsManager;
using Honeycomb.OpenTelemetry;
using Microsoft.OpenApi.Models;
using MultiTenantApi.Api.Middleware;
using MultiTenantApi.Application.Categories.Interfaces;
using MultiTenantApi.Application.Data.Interfaces;
using MultiTenantApi.Application.Products;
using MultiTenantApi.Application.Products.Interfaces;
using MultiTenantApi.Application.Secrets.Interfaces;
using MultiTenantApi.Application.Tenants.Interfaces;
using MultiTenantApi.Infrastructure.Categories;
using MultiTenantApi.Infrastructure.Data;
using MultiTenantApi.Infrastructure.Products;
using MultiTenantApi.Infrastructure.Secrets;
using MultiTenantApi.Infrastructure.Tenants;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITenantContext, TenantContext>();
builder.Services.AddSingleton<IAmazonSecretsManager, AmazonSecretsManagerClient>();
builder.Services.AddSingleton<ISecretsManagerService, SecretsManagerService>();
builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddControllers();

// Add Honeycomb OpenTelemetry
builder.Services.AddOpenTelemetry().WithTracing(otelBuilder =>
{
    otelBuilder
        .AddHoneycomb(builder.Configuration.GetSection("Honeycomb").Get<HoneycombOptions>())
        .AddAspNetCoreInstrumentation()
        .AddSqlClientInstrumentation(options =>
        {
            options.SetDbStatementForText = true;
            options.RecordException = true;
        });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MultiTenantApi", Version = "v1" });
    c.AddSecurityDefinition("X-Tenant-Id", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X-Tenant-Id",
        Type = SecuritySchemeType.ApiKey,
        Description = "Tenant ID"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "X-Tenant-Id"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<TenantResolverMiddleware>();

app.MapControllers();

app.Run();
