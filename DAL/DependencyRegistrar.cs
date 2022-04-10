using Core;
using DAL.Abstractions.Interfaces;
using DAL.Repository;
using DAL.Worker;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class DependencyRegistrar
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppContext>();
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            // services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositoryDb<>));
            services.AddScoped<IJsonWorker, JsonWorker>();
        }
    }
}
