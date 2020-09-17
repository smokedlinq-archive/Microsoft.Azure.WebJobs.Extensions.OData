using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.Azure.WebJobs.Extensions.OData
{
    internal static class ODataWebJobsStartupExtensions
    {
        public static IWebJobsBuilder AddOData(this IWebJobsBuilder builder)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.Services.AddSingleton(serviceProvider =>
                new ODataContext(() => serviceProvider.GetServices<ConfigureODataConventionModelBuilder>()));

            builder.Services.AddSingleton<IBindingProvider, ODataBindingProvider>();

            return builder;
        }
    }
}
