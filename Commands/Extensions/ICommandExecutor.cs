using MediatorAndHangfireExample.Models;

namespace MediatorAndHangfireExample.Commands.Extensions;

public interface ICommandExecutor
{
    public Task ExecuteCommand(MediatorSerializedObject mediatorSerializedObject);
}