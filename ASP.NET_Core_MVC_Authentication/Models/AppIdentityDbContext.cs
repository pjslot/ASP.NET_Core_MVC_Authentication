//класс идентифицируемого DB контекста
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_MVC_Authentication.Models
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base (options) 
        {
           //создание базы данных, если её не существует
            Database.EnsureCreated();
        }
    }
}
