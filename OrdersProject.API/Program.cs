using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrdersProject.API.Endpoints;
using OrdersProject.Application;
using OrdersProject.Application.Common;
using OrdersProject.Application.Interfaces;
using OrdersProject.Domain.Interfaces;
using OrdersProject.Infrastructure.Persistence;
using OrdersProject.Infrastructure.Persistence.Repositories;
using OrdersProject.Infrastructure.Services;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// EF Core + MySQL
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))
));

// CQRS + Validators
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);

// DI
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IInboundEmailRepository, InboundEmailRepository>();

// Minimal API + Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// IMAP + SMTP
builder.Services.Configure<EmailSettings>(options =>
{
    builder.Configuration.GetSection("EmailSettings").Bind(options);
    options.Username = Environment.GetEnvironmentVariable("EMAIL_USERNAME") ?? "";
    options.Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? "";
});

builder.Services.AddScoped<ImapService>();
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();

// OpenAI
builder.Services.Configure<OpenAiSettings>(options =>
{
    builder.Configuration.GetSection("OpenAiSettings").Bind(options);
    options.ApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "";

});

builder.Services.AddHttpClient<IMailParserService, OpenAiMailParserService>();

// Migration
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    db.Database.Migrate();
}

// Mail parser
builder.Services.AddHostedService<InboundEmailProcessingService>();


var app = builder.Build();


// Auto migracja
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Https only if not dev
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
// app.UseAuthorization();

app.MapOrders();
app.Run();
