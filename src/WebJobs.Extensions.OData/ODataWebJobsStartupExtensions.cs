using System;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OData;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.WebJobs.Extensions.OData
{
    internal static class ODataWebJobsStartupExtensions
    {
        public static IWebJobsBuilder AddOData(this IWebJobsBuilder builder)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));
            builder.Services.AddSingleton(new ODataContext());
            builder.Services.AddSingleton<IBindingProvider, ODataBindingProvider>();
            return builder;
        }
    }
}
