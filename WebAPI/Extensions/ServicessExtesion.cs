using DAL;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services;
using System.Reflection;

namespace WebAPI.Extensions
{
    public static class ServicessExtesion
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EmployeeDBContext>(options =>
                                                   options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IEmployeeService, EmployeeService>();

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Description = "Employees Web Api For Employees Management.",
                    Title = "Employees Web Api"
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            return services;
        }
    }
}
