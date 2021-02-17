using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PriceEstimation.JwellaryShop.WebApi.Context;
using PriceEstimation.JwellaryShop.WebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceEstimation.JwellaryShop.WebApi.Helpers
{
    public static class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var ctx = new EstimationDBContext(serviceProvider.GetRequiredService<DbContextOptions<EstimationDBContext>>()))
            {
                if(ctx.Users.Any())
                {
                    return;
                }

                ctx.Users.AddRange(new User
                {
                    Id = 1,
                    FirstName = "Privileged",
                    LastName = "User",
                    Username = "admin",
                    Password = "admin",
                    Role = Role.Privileged
                },
                new User
                {
                    Id = 2,
                    FirstName = "Regular",
                    LastName = "User",
                    Username = "user",
                    Password = "user",
                    Role = Role.Regular
                });

                ctx.SaveChanges();
            }
        }
    }
}
