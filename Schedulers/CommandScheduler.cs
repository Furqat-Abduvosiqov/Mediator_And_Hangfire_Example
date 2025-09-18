using Hangfire;
using MediatorAndHangfireExample.Commands.Extensions;
using MediatorAndHangfireExample.Models;
using MediatR;
using Newtonsoft.Json;

namespace MediatorAndHangfireExample.Schedulers;

public class CommandScheduler : ICommandScheduler
{
    private readonly ICommandExecutor _commandExecutor;

    public CommandScheduler(ICommandExecutor commandExecutor)
    {
        _commandExecutor = commandExecutor;
    }

    public string SendNow(IRequest request, string? description = null)
    {
        var mediatorSerializedObject = SerializeObject(request, description);
        return BackgroundJob.Enqueue(() => _commandExecutor.ExecuteCommand(mediatorSerializedObject));
    }

    public string SendNow(
        IRequest request,
        string parentJobId,
        JobContinuationOptions continuationOption,
        string? description = null)
    {
        var mediatorSerializedObject = SerializeObject(request, description);
        return BackgroundJob.ContinueWith(
            parentJobId,
            () => _commandExecutor.ExecuteCommand(mediatorSerializedObject),
            continuationOption);
    }

    public void Schedule(IRequest request, DateTimeOffset scheduleAt, string? description = null)
    {
        var mediatorSerializedObject = SerializeObject(request, description);
        BackgroundJob.Schedule(() => _commandExecutor.ExecuteCommand(mediatorSerializedObject), scheduleAt);
    }

    public void ScheduleRecurring(IRequest request, string name, string cronExpression, string? description = null)
    {
        var mediatorSerializedObject = SerializeObject(request, description);
        RecurringJob.AddOrUpdate(
            name,
            () => _commandExecutor.ExecuteCommand(mediatorSerializedObject),
            cronExpression,
            TimeZoneInfo.Local);
    }

    private MediatorSerializedObject SerializeObject(IRequest mediatorObject, string? description)
    {
        string fullTypeName = mediatorObject.GetType().AssemblyQualifiedName!; // not just FullName
        string data = JsonConvert.SerializeObject(mediatorObject, new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            ContractResolver = new PrivateJsonDefaultContractResolver()
        });

        return new MediatorSerializedObject(fullTypeName, data, description ?? string.Empty);
    }
}