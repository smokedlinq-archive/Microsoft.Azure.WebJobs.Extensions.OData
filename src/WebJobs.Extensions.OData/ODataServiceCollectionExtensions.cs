using System;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OData;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ODataServiceCollectionExtensions
    {
        public static IServiceCollection AddOData(this IServiceCollection services, Action<ODataConventionModelBuilder> configure)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            services.AddSingleton(_ => new ConfigureODataConventionModelBuilder(configure));
            return services;
        }
    }
}
