using System;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Query.Validators;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.UriParser;

namespace Microsoft.Azure.WebJobs.Extensions.OData
{
    internal class ODataContext
    {
        public ODataContext()
        {
            var services = new ServiceCollection();

            services.AddMvcCore();
            services.AddOData();
            services.AddTransient<ODataUriResolver>();
            services.AddTransient<ODataQueryValidator>();
            services.AddTransient<TopQueryValidator>();
            services.AddTransient<FilterQueryValidator>();
            services.AddTransient<SkipQueryValidator>();
            services.AddTransient<OrderByQueryValidator>();
            services.AddTransient<CountQueryValidator>();
            services.AddTransient<SelectExpandQueryValidator>();
            services.AddTransient<SkipTokenQueryValidator>();
            
            Services = services.BuildServiceProvider();

            RouteBuilder = new RouteBuilder(new ApplicationBuilder(Services));
            RouteBuilder
                .Count()
                .Expand()
                .Filter()
                .MaxTop(null)
                .OrderBy()
                .Select()
                .SkipToken();
            RouteBuilder.EnableDependencyInjection();
        }

        public IServiceProvider Services { get; private set; }

        public RouteBuilder RouteBuilder { get; private set; }
    }
}