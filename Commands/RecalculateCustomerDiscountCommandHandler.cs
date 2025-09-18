using MediatR;

namespace MediatorAndHangfireExample.Commands;

public class RecalculateCustomerDiscountCommandHandler : IRequestHandler<RecalculateCustomerDiscountCommand>
{
    public async Task Handle(RecalculateCustomerDiscountCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[START] Recalculating discounts for Customer {request.CustomerId}");

        // Simulate long-running DB operation (3–5 seconds)
        await Task.Delay(TimeSpan.FromSeconds(new Random().Next(3, 6)), cancellationToken);

        Console.WriteLine($"[DONE] Discounts recalculated for Customer {request.CustomerId}");
    }
}