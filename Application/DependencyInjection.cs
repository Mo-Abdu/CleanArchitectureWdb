using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
namespace Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(assembly));

            return services;
        }
    }
}
