using System;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OData;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.WebJobs.Extensions.OData
{
    internal class ConfigureODataConventionModelBuilder
    {
        private readonly Action<ODataConventionModelBuilder> _callback;

        public ConfigureODataConventionModelBuilder(Action<ODataConventionModelBuilder> configure)
            => _callback = configure ?? throw new ArgumentNullException(nameof(configure));

        public void Configure(ODataConventionModelBuilder builder)
            => _callback(builder);
    }
}
