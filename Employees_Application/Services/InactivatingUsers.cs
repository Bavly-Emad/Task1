using Employees_Infrastructure.DataContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees_Application.Services
{
    public class InactivatingUsers : BackgroundService
    {
        private readonly DBContext cntxt;
        private readonly IServiceProvider srvcp;
        private readonly ILogger<InactivatingUsers> lger;
        public InactivatingUsers(DBContext dBContext, IServiceProvider serviceProvider, ILogger<InactivatingUsers> logger)
        {
            cntxt = dBContext;
            lger = logger;
            srvcp = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                lger.LogInformation("Checking for incomplete registrations...");

                using (var scope = srvcp.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DBContext>();

                    var incompleteEmployees = dbContext.Employee
                        .Where(e => string.IsNullOrEmpty(e.Gender) || string.IsNullOrEmpty(e.Marital_Status))
                        .ToList();

                    foreach (var employee in incompleteEmployees)
                    {
                        employee.Inactive = true;
                    }

                    await dbContext.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Runs every hour
            }
        }
        /*public async Task InavtivatingUsers()
        {
            lger.LogInformation("Inactivating employees with incomplete registration data.");

            var incompleteEmployees = cntxt.Employee
                .Where(e => string.IsNullOrEmpty(e.Gender) || string.IsNullOrEmpty(e.Marital_Status))
                .ToList();

            foreach (var employee in incompleteEmployees)
            {
                employee.Inactive = true;
            }

            await cntxt.SaveChangesAsync();
            lger.LogInformation("Inactivated {Count} employees.", incompleteEmployees.Count);
        }*/
    }
}
