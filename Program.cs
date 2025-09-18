using System.Reflection;
using Hangfire;
using MediatorAndHangfireExample.Commands.Extensions;
using MediatorAndHangfireExample.Schedulers;
using MediatorAndHangfireExample.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Add Hangfire with in-memory storage for demo
builder.Services.AddHangfire(config => config.UseInMemoryStorage());
builder.Services.AddHangfireServer();

// Add MediatR (scan current assembly for handlers)
builder.Services.AddMediatR(opt =>
{
    opt.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
});

// Register our scheduler & executor
builder.Services.AddSingleton<ICommandScheduler, CommandScheduler>();
builder.Services.AddSingleton<ICommandExecutor, CommandExecutor>();

// Add background worker
builder.Services.AddHostedService<DiscountRecalculationWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHangfireDashboard(); // optional, UI at /hangfire

app.MapGet("/", () => "Hangfire + MediatR demo running...");

app.UseHttpsRedirection();

app.Run();
