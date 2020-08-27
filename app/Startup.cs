using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;

[assembly: FunctionsStartup(typeof(Company.Function.Startup))]

namespace Company.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));
            builder.Services.AddDbContext<AppContext>(options => options.UseInMemoryDatabase(nameof(AppContext)));
            builder.Services.AddOData(builder =>
            {
                builder.EntitySet<Department>("Departments");
                builder.EntitySet<Product>("Products");
            });
        }
    }
}