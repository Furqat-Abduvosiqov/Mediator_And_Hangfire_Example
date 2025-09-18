using MediatR;

namespace MediatorAndHangfireExample.Commands;

public record RecalculateCustomerDiscountCommand(Guid CustomerId) : IRequest;
