using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Company.Function
{
    public class GetProductsHttp
    {
        private readonly AppContext _context;

        public GetProductsHttp(AppContext context)
            => _context = context ?? throw new ArgumentNullException(nameof(context));

        [EnableQuery(MaxTop = 100, AllowedQueryOptions = AllowedQueryOptions.All)]
        [FunctionName(nameof(GetProductsHttp))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products")] HttpRequest req,
            ODataQueryOptions<Product> odata,
            ILogger log)
        {
            var results = await odata.ApplyTo(_context.Products).Cast<dynamic>().ToListAsync().ConfigureAwait(false);
            return new OkObjectResult(results);
        }
    }
}
