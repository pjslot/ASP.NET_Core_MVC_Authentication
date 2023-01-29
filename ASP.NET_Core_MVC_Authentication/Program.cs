

using ASP.NET_Core_MVC_Authentication.Infrastructure;
using ASP.NET_Core_MVC_Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//добавление контекста EF и других зависимостей в коллекцию служб
string cnStr = "Server = (localdb)\\MSSQLLocalDB; Database = IdentityUsers; Trusted_Connection = True; MultipleActiveResultSets = True";
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(cnStr));

//builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders(); - перенесено из-за дублирования

builder.Services.AddAuthentication();

builder.Services.AddIdentity<AppUser, IdentityRole>
    (options =>
    {
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    
    }).AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders()
    ;


//регистрация зависимости кастомного класса проверки паролей (отменяет штатные средства)
builder.Services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordValidator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
}
app.UseAuthentication();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
