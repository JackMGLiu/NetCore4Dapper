using System;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiCore.Infrastructure.Data
{
    public static class DapperDbContextServiceCollectionExtensions
    {
        public static IServiceCollection AddDapperDBContext<T>(this IServiceCollection services, Action<DapperDbContextOptions> setupAction) where T : DapperDbContext
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddOptions();
            services.Configure(setupAction);
            services.AddScoped<DapperDbContext, T>();
            services.AddScoped<IUnitOfWorkFactory, DapperUnitOfWorkFactory>();

            return services;
        }
    }
}