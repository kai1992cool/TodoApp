using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Data.DBContext;
using Data.Interfaces;
using To_Do.Data.Repo;
using Data.Repo;

namespace Data
{
    public static class ServiceExtensions
    {
        public static void ConfigureInfrastructure(this IHostApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IToDoItemRepo, ToDoItemRepo>();
            builder.Services.AddDbContext<ToDoDBContext>(options => options.UseSqlServer(configuration.GetConnectionString("DBConeection")));
        }
    }
}
