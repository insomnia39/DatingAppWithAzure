﻿using DatingApp.BLL.JWT;
using DatingApp.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.FrontEndAPI.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            string databaseName = Profile._containerName;
            var connectionString = config.GetConnectionString("CosmosDb");

            services.AddDbContext<ProfileContext>(opt =>
            {
                opt.UseCosmos(connectionString, databaseName);
            });

            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}