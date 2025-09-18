using MediatorAndHangfireExample.Commands;
using MediatorAndHangfireExample.Schedulers;

namespace MediatorAndHangfireExample.Workers;

public class DiscountRecalculationWorker : BackgroundService
{
    private readonly ILogger<DiscountRecalculationWorker> _logger;
    private readonly ICommandScheduler _scheduler;
    private readonly Random _random = new();
    
    public DiscountRecalculationWorker(ILogger<DiscountRecalculationWorker> logger, ICommandScheduler scheduler)
    {
        _logger = logger;
        _scheduler = scheduler;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DiscountRecalculationWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var customerId = Guid.NewGuid();
            var command = new RecalculateCustomerDiscountCommand(customerId);

            _logger.LogInformation("Scheduling discount recalculation for {CustomerId}", customerId);

            // Send to Hangfire immediately (async execution)
            _scheduler.SendNow(command, $"Recalculate discounts for {customerId}");

            // Wait a random time between 5–10 seconds before scheduling next one
            await Task.Delay(TimeSpan.FromSeconds(_random.Next(5, 11)), stoppingToken);
        }
    }
}
