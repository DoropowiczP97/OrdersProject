using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrdersProject.API.Endpoints;
using OrdersProject.Application;
using OrdersProject.Application.Common;
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


// IMAP
builder.Services.Configure<ImapSettings>(options =>
{
    builder.Configuration.GetSection("ImapSettings").Bind(options);

    options.Username = Environment.GetEnvironmentVariable("IMAP_USERNAME") ?? "";
    options.Password = Environment.GetEnvironmentVariable("IMAP_PASSWORD") ?? "";
});

builder.Services.AddScoped<ImapService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var imap = scope.ServiceProvider.GetRequiredService<ImapService>();
    await imap.FetchUnreadEmailsAsync(CancellationToken.None);
}

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

app.UseHttpsRedirection();
// app.UseAuthorization();

app.MapOrders();
app.Run();
