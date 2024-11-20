using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using MediatR;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(Configuration => Configuration.RegisterServicesFromAssembly(assembly));

            //services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
