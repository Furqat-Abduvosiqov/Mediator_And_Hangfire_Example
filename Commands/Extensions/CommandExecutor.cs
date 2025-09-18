using System.ComponentModel;
using MediatorAndHangfireExample.Models;
using MediatR;
using Newtonsoft.Json;

namespace MediatorAndHangfireExample.Commands.Extensions;

public class CommandExecutor : ICommandExecutor
{
    private readonly IMediator _mediator;

    public CommandExecutor(IMediator mediator)
    {
        _mediator = mediator;
    }

    [DisplayName("Processing command {0}")]
    public async Task ExecuteCommand(MediatorSerializedObject mediatorSerializedObject)
    {
        if (mediatorSerializedObject is null)
            throw new ArgumentNullException(nameof(mediatorSerializedObject));

        // Try to resolve type directly from AssemblyQualifiedName
        var type = Type.GetType(mediatorSerializedObject.FullTypeName);

        // Fallback: search across all loaded assemblies
        if (type is null)
        {
            type = AppDomain.CurrentDomain
                .GetAssemblies()
                .Select(a => a.GetType(mediatorSerializedObject.FullTypeName))
                .FirstOrDefault(t => t != null);
        }

        if (type is null)
            throw new InvalidOperationException(
                $"Cannot find type '{mediatorSerializedObject.FullTypeName}' in loaded assemblies.");

        var request = JsonConvert.DeserializeObject(mediatorSerializedObject.Data, type) as IRequest;
        if (request is null)
            throw new InvalidOperationException(
                $"Deserialization failed for type '{mediatorSerializedObject.FullTypeName}'.");

        await _mediator.Send(request);
    }
}
