using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrdersProject.Application.Common;
using OrdersProject.Application.Interfaces;
using OrdersProject.Domain.Interfaces;
using OrdersProject.Infrastructure.Persistence;
using OrdersProject.Infrastructure.Persistence.Repositories;
using OrdersProject.Infrastructure.Services;

namespace OrdersProject.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
                                                                IConfiguration configuration)
    {
        // DbContext + MySQL
        services.AddDbContext<OrderDbContext>(options =>
            options.UseMySql(
                configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 36)))
        );

        // Repositories
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IInboundEmailRepository, InboundEmailRepository>();

        // IMAP/SMTP
        services.Configure<EmailSettings>(opts =>
        {
            configuration.GetSection("EmailSettings").Bind(opts);
            opts.Username = Environment.GetEnvironmentVariable("EMAIL_USERNAME") ?? "";
            opts.Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? "";
        });
        services.AddScoped<ImapService>();
        services.AddScoped<IEmailSenderService, EmailSenderService>();

        // 4) OpenAI Mail Parser client
        services.Configure<OpenAiSettings>(opts =>
        {
            configuration.GetSection("OpenAiSettings").Bind(opts);
            opts.ApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "";
        });

        services.AddHttpClient<IMailParserService, OpenAiMailParserService>();

        // Background worker
        services.AddHostedService<InboundEmailProcessingService>();

        return services;
    }

}
