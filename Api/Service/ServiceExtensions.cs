using Service.Interfaces;
using Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

namespace Service
{
    public static class ServiceExtensions
    {
        public static void ConfigureApplication(this IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUser, UserServices>();
            builder.Services.AddScoped<IToDoItem, ToDoItemServices>();
            builder.Services.AddScoped<ITokenService, TokenServices>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAutoMapper(typeof(ServiceExtensions));
            builder.Services.AddControllers().AddNewtonsoftJson();
        }
    }
}
