using ECommerce.Application.Services;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ECommerce.Infrastructure.Data.Repositories;
using ECommerce.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     var loggerFactory = services.GetRequiredService<ILoggerFactory>();
//     try
//     {
//         var context = services.GetRequiredService<AppDbContext>();
//         
//         // دي الخطوة اللي بتعمل الـ Migration تلقائياً لو مش معمولة
//         await context.Database.MigrateAsync(); 
//         
//         // هنا بننادي على الكلاس اللي كلاود ادهولك
//         await AppDbContextSeed.SeedAsync(context);
//     }
//     catch (Exception ex)
//     {
//         var logger = loggerFactory.CreateLogger<Program>();
//         logger.LogError(ex, "An error occurred during migration or seeding");
//     }
// }

app.Run();