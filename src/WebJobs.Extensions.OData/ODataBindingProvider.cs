using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Extensions.OData;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.WebJobs.Extensions.OData
{
    internal class ODataBindingProvider : IBindingProvider
    {
        private readonly ODataContext _context;
        
        public ODataBindingProvider(ODataContext context)
            => _context = context ?? throw new ArgumentNullException(nameof(context));

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));
            
            var attribute = context.Parameter.Member.GetCustomAttribute<EnableQueryAttribute>(inherit: false);
            
            if (attribute != null && context.Parameter.ParameterType.IsGenericType && context.Parameter.ParameterType.GetGenericTypeDefinition() == typeof(ODataQueryOptions<>))
            {
                return Task.FromResult<IBinding>(new ODataBinding(_context, attribute, context.Parameter.ParameterType.GenericTypeArguments[0]));
            }

            return Task.FromResult<IBinding>(null);
        }
    }
}
