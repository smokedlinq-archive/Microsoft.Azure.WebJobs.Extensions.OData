using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Query.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace Microsoft.Azure.WebJobs.Extensions.OData
{
    internal class ODataContext : IDisposable
    {
        private readonly Lazy<IEdmModel> _model;

        ~ODataContext() => Dispose(false);

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Services.Dispose();
            }
        }

        public ODataContext(Func<IEnumerable<ConfigureODataConventionModelBuilder>> conventions)
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

            var routeBuilder = new RouteBuilder(new ApplicationBuilder(Services));
            routeBuilder
                .Count()
                .Expand()
                .Filter()
                .MaxTop(null)
                .OrderBy()
                .Select()
                .SkipToken();
            routeBuilder.EnableDependencyInjection();

            _model = new Lazy<IEdmModel>(() =>
            {
                var builder = new ODataConventionModelBuilder(Services);

                foreach(var convention in conventions())
                {
                    convention.Configure(builder);
                }

                return builder.GetEdmModel();
            });
        }

        public ServiceProvider Services { get; private set; }

        public IEdmModel Model => _model.Value;
    }
}