using DAL;

namespace WebAPI.Extensions
{
    public static class WebApplicationExtension
    {
        public static WebApplication MigrateDatabase(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                using (var employeeContext = services.GetRequiredService<EmployeeDBContext>())
                {
                    try
                    {
                        DbInitializer.Initialize(employeeContext);
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while seeding the database.");
                    }
                }
            }

            return webApp;
        }

        public static WebApplication UseErrorHanlder(this WebApplication webApp)
        {
            if (webApp.Environment.IsDevelopment())
            {
                webApp.UseExceptionHandler("/error-development");
            }
            else
            {
                webApp.UseExceptionHandler("/error");
            }

            return webApp;
        }
    }
}
