using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData;
using Microsoft.EntityFrameworkCore;

namespace Company.Function
{
    public class GetDepartmentsHttp
    {
        private readonly AppContext _context;

        public GetDepartmentsHttp(AppContext context)
            => _context = context ?? throw new ArgumentNullException(nameof(context));

        [EnableQuery(MaxTop = 100, AllowedQueryOptions = AllowedQueryOptions.All)]
        [FunctionName(nameof(GetDepartmentsHttp))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "departments")] HttpRequest req,
            ODataQueryOptions<Department> odata,
            ILogger log)
        {
            var results = await odata.ApplyTo(_context.Departments).Cast<dynamic>().ToListAsync().ConfigureAwait(false);
            return new OkObjectResult(results);
        }
    }
}
