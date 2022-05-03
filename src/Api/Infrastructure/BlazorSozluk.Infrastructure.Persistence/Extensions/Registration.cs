using BlazorSozluk.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Infrastructure.Persistence.Extentions
{
    public static class Registration
    {
        public static IServiceCollection AddInfrustructureRegistration (this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<BlazorSozlukContext>(conf =>
            {
                //   var connStr=configuration["BlazorSozlukDbConnectionString"].ToString();
              

                conf.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), opt =>
                {
                    opt.EnableRetryOnFailure();
                });
            });
            //var seedData = new SeedData();
            //seedData.SeedAsync(configuration).GetAwaiter().GetResult();
            // new SeedData().SeedAsync(configuration).GetAwaiter().GetResult();
            return services;
        }
    }
}
