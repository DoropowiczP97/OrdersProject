using OrdersProject.API.Endpoints;
using OrdersProject.Application.Extensions;
using OrdersProject.Infrastructure.Extensions;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Register services Application and Infrastructure
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

// Minimal API + Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Auto migration of the database
await app.MigrateDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

// Endpoints mapping
app.MapOrders();

await app.RunAsync();
