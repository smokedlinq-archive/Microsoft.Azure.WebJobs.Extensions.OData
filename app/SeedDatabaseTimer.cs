using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Company.Function
{
    public class SeedDatabaseTimer
    {
        private readonly AppContext _context;

        public SeedDatabaseTimer(AppContext context)
            => _context = context ?? throw new ArgumentNullException(nameof(context));

        [FunctionName(nameof(SeedDatabaseTimer))]
        public async Task Run([TimerTrigger("0 0 1 1 *", RunOnStartup = true)] TimerInfo timer)
        {
            if (!await _context.Products.AnyAsync().ConfigureAwait(false))
            {
                _context.Products.AddRange(
                    new Product
                    {
                        Sku = 1,
                        Name = "Apple"
                    },
                    new Product
                    {
                        Sku = 2,
                        Name = "Lettuce"
                    },
                    new Product
                    {
                        Sku = 3,
                        Name = "Potato"
                    });

                _context.SaveChanges();
            }
        }
    }
}