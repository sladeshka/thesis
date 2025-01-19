using Microsoft.EntityFrameworkCore;
using SalesVentilationEquipment.Server;
using SalesVentilationEquipment.Server.Data;
using SalesVentilationEquipment.Server.Models;
using SalesVentilationEquipment.Server.Repositories;
using SalesVentilationEquipment.Server.Services;

var builder = WebApplication.CreateBuilder(args);

#region Adding a configurations
IHostEnvironment env = builder.Environment;
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .Build();
#endregion

#region Initialization configuration class
AppSettings.Initialize(configuration);
#endregion

#region Adding a database connection
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(AppSettings.DBConnectionString));
#endregion

#region Adding a log
builder.Services.AddLogging();
#endregion

#region Adding a repositories
builder.Services.AddScoped<Repository<Cart>>();
builder.Services.AddScoped<Repository<Contractor>>();
builder.Services.AddScoped<Repository<ContractorAndStore>>();
builder.Services.AddScoped<Repository<Order>>();
builder.Services.AddScoped<Repository<Product>>();
builder.Services.AddScoped<Repository<ProductInCart>>();
builder.Services.AddScoped<Repository<Remains>>();
builder.Services.AddScoped<Repository<Store>>();
builder.Services.AddScoped<Repository<StoreAndWarehouse>>();
builder.Services.AddScoped<Repository<Warehouse>>();
#endregion

#region Adding a web services
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<ContractorService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProductInCartService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<StoreService>();
builder.Services.AddScoped<WarehouseService>();

#endregion


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

#region Adding a database migration function
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    using (var context = services.GetRequiredService<ApplicationDbContext>())
    {
        context.Database.Migrate();
    }
}
#endregion

app.UseCors("AllowAll");
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
