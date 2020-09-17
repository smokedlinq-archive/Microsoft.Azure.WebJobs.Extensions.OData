using Microsoft.Azure.WebJobs.Extensions.OData;
using Microsoft.Azure.WebJobs.Hosting;

#pragma warning disable CA1812

[assembly: WebJobsStartup(typeof(ODataWebJobsStartup))]

namespace Microsoft.Azure.WebJobs.Extensions.OData
{
    internal class ODataWebJobsStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddOData();
        }
    }
}
