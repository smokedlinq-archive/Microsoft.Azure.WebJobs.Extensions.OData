using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace Microsoft.Azure.WebJobs.Extensions.OData
{
    internal class ODataBindingValueProvider : IValueProvider
    {
        private readonly object _value;

        public ODataBindingValueProvider(object value)
            => _value = value ?? throw new ArgumentNullException(nameof(value));

        public Type Type => _value.GetType();

        public Task<object> GetValueAsync()
            => Task.FromResult(_value);

        public string ToInvokeString()
            => "odata";
    }
}