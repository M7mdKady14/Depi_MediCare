using Clinic.Infrastructure.Contracts;
using Clinic.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clinic.Infrastructure
{
    public static class ModuleInfrastructureDependencies 
    {
        public static IServiceCollection AddModuleInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(TableRepository<>));
            services.AddScoped(typeof(IViewRepository<>), typeof(ViewRepository<>));
            services.AddScoped<IUnitOfWork,UnitOfWork>();

            return services;
        }
    }
}
