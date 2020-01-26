using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.OData;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData;

namespace Company.Function
{
    public static class HttpTriggerCSharp
    {
        [EnableQuery(MaxTop = 100, AllowedQueryOptions = AllowedQueryOptions.Select | AllowedQueryOptions.Filter | AllowedQueryOptions.Top)]
        [FunctionName("products")]
        public static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ODataQueryOptions<Product> odata,
            ILogger log)
        {
            var data = new List<Product>
            {
                new Product
                {
                    Sku = 1,
                    Name = "Apple"
                },
                new Product
                {
                    Sku = 2,
                    Name = "Lettuce"
                }
            };

            return Task.FromResult<IActionResult>(new OkObjectResult(odata.ApplyTo(data.AsQueryable())));
        }
    }

    public class Product
    {
        public int Sku { get; set; }
        public string Name { get; set; }
    }
}
