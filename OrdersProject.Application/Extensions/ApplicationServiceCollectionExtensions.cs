using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace OrdersProject.Application.Extensions;


public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // CQRS
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);

        return services;
    }
}
