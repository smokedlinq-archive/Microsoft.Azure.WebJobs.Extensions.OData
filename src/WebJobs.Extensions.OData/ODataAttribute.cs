using Microsoft.Azure.WebJobs.Description;
using System;

namespace Microsoft.Azure.WebJobs.Extensions.OData
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ODataAttribute : Attribute
    {
    }
}
