using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using ProductCatalogAPI.Repository;
using ProductCatalogAPI.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product Catalog API",
        Version = "v1"
    });
});

//Connection string
var mongoHost = Environment.GetEnvironmentVariable("DATABASE_HOST");
var mongoPort = Environment.GetEnvironmentVariable("DATABASE_PORT");
var mongoDatabaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");
var mongoUsername = Environment.GetEnvironmentVariable("DATABASE_USERNAME");
var mongoPassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

var mongoConnectionString = $"mongodb://{mongoUsername}:{mongoPassword}@{mongoHost}:{mongoPort}";

var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);
builder.Services.AddScoped(provider => mongoDatabase);

builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<SupplierRepository>();
builder.Services.AddScoped<ProductRepository>();

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<SupplierService>();
builder.Services.AddScoped<ProductService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        options.RoutePrefix = string.Empty; // Makes Swagger UI available at the root (localhost:8080)
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
