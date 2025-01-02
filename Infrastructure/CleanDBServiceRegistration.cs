using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation
{
    public static class CleanDBServiceRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
#if DEBUG
            services.AddDbContext<CleanDBContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("LOCAL"), opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)));
#else
           // SET ConnectionString From App Service
            services.AddDbContext<PersistenceDbContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("Connection"), opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)));
#endif


            return services;
        }
    }
}
