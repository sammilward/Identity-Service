using System;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Data
{
    public class SeedDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.EnsureCreated();
        }
    }
}
