using System;
using Microsoft.Azure.WebJobs.Description;

namespace Microsoft.Azure.WebJobs.Extensions.OData
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ODataAttribute : Attribute
    {
    }
}
