using EletronicShop.Domain.Repositories;
using EletronicShop.Domain.Services;
using EletronicShop.Domain.Services.Interfaces;
using EletronicShop.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EletronicShop.CrossCutting.IoC
{
    public static class DependencyResolver
    {
        public static void AddDependencyResolver(this IServiceCollection services)
        {
            RegisterServices(services);
            RegisterRepositories(services);
            RegisterDataBase(services);
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
        }

        private static void RegisterDataBase(IServiceCollection services)
        {
            var connectionString = "Server=localhost, 1433;Database=master;User Id=sa;Password=1q2w3e4r@#$;";
            services.AddScoped<IDbConnection, SqlConnection>(scope =>
            {
                var connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            });
        }
    }
}
