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
                _context.Departments.AddRange(
                    new Department
                    {
                        Id = 1,
                        Name = "Fruits"
                    },
                    new Department
                    {
                        Id = 2,
                        Name = "Vegetables"
                    }
                );
                _context.Products.AddRange(
                    new Product
                    {
                        Sku = 1,
                        DepartmentId = 1,
                        Name = "Apple"
                    },
                    new Product
                    {
                        Sku = 2,
                        DepartmentId = 1,
                        Name = "Orange"
                    },
                    new Product
                    {
                        Sku = 3,
                        DepartmentId = 2,
                        Name = "Lettuce"
                    },
                    new Product
                    {
                        Sku = 4,
                        DepartmentId = 2,
                        Name = "Potato"
                    });

                _context.SaveChanges();
            }
        }
    }
}