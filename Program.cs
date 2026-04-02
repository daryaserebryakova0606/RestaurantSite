using Microsoft.EntityFrameworkCore;
using RestaurantSite.Data;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контекст базы данных SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=restaurant.db"));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Создаем базу данных при первом запуске
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();