using Azure.Core;
using Azure.Identity;
using Dotnet8WebAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product API",
        Version = "3.0.0", // Ensure
        Description = "A simple CRUD API for managing products"
    });
});


/*// Configure Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));*/

// var connectionString = "Server=tcp:sqlserver-dotnet8webapi-uscentral-dev-001.database.windows.net;Database=GDW2;Encrypt=True;";

var tokenCredential = new DefaultAzureCredential();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
            );
        });
});



var app = builder.Build();

// Enable Swagger in all environments
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
    c.RoutePrefix = "swagger"; // Serve Swagger UI at the root "/"
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
