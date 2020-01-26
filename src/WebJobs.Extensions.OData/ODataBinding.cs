using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;

namespace Microsoft.Azure.WebJobs.Extensions.OData
{
    internal class ODataBinding : IBinding
    {
        private readonly ODataContext _context;
        private readonly EnableQueryAttribute _attribute;
        private readonly Func<HttpRequest, ODataQueryOptions> _oDataQueryOptionsFactory;

        public ODataBinding(ODataContext context, EnableQueryAttribute attribute, Type type)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _attribute = attribute ?? throw new ArgumentNullException(nameof(attribute));
            _oDataQueryOptionsFactory = BuildODataQueryOptionsFactory(type ?? throw new ArgumentNullException(nameof(type)));
        }

        public bool FromAttribute => false;

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            var request = value as HttpRequest ?? throw new InvalidOperationException($"{nameof(value)} must be an {nameof(HttpRequest)}");
            var odataQuery = _oDataQueryOptionsFactory(request);
            
            _attribute.ValidateQuery(request, odataQuery);
            
            var provider = new ODataBindingValueProvider(odataQuery);
            return Task.FromResult<IValueProvider>(provider);
        }

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));
            var request = context.BindingData["$request"] as HttpRequest;
            var http = new DefaultHttpContext
            {
                RequestServices = _context.Services
            };

            var odataRequest = new DefaultHttpRequest(http)
            {
                Method = request.Method,
                Host = request.Host,
                Path = request.Path,
                QueryString = request.QueryString
            };

            return BindAsync(odataRequest, context.ValueContext);
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor
                {
                    Name = "odata"
                };
        }

        private Func<HttpRequest, ODataQueryOptions> BuildODataQueryOptionsFactory(Type type)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));

            var builder = new ODataConventionModelBuilder(_context.Services);
            builder.AddEntityType(type);
            var model = builder.GetEdmModel();
            var context = new ODataQueryContext(model, type, new AspNet.OData.Routing.ODataPath());
            
            var queryOptionsType = typeof(ODataQueryOptions<>).MakeGenericType(type);
            var queryOptionsCtor = queryOptionsType.GetConstructor(new[] { typeof(ODataQueryContext), typeof(HttpRequest) });
            var queryOptionsParams = new ParameterExpression[]
            {
                Expression.Parameter(typeof(ODataQueryContext), "context"),
                Expression.Parameter(typeof(HttpRequest), "request")
            };
            var lambda = Expression.Lambda<Func<ODataQueryContext, HttpRequest, ODataQueryOptions>>(Expression.New(queryOptionsCtor, queryOptionsParams), queryOptionsParams).Compile();

            return (request) => lambda.Invoke(context, request);
        }
    }
}