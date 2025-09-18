using Hangfire;
using MediatR;

namespace MediatorAndHangfireExample.Schedulers;

public interface ICommandScheduler
{
    public string SendNow(IRequest request, string? description = null);

    public string SendNow(IRequest request, string parentJobId, JobContinuationOptions continuationOption,
        string? description = null);

    public void Schedule(IRequest request, DateTimeOffset scheduleAt, string? description = null);

    public void ScheduleRecurring(IRequest request, string name, string cronExpression, string? description = null);
}
