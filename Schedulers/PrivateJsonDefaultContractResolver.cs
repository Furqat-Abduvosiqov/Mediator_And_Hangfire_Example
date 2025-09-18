using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MediatorAndHangfireExample.Schedulers;

/// <summary>
/// Custom contract resolver that serializes private setters / non-public members.
/// </summary>
internal class PrivateJsonDefaultContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
    {
        var prop = base.CreateProperty(member, memberSerialization);

        // Allow setting private setters during deserialization
        if (!prop.Writable)
        {
            var property = member as System.Reflection.PropertyInfo;
            if (property?.GetSetMethod(true) != null)
            {
                prop.Writable = true;
            }
        }

        return prop;
    }
}