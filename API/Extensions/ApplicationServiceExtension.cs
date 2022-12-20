using DatingApp.BLL.Helpers;
using DatingApp.BLL.JWT;
using DatingApp.BLL.MessageGroupManagement;
using DatingApp.BLL.MessageManagement;
using DatingApp.BLL.Photo;
using DatingApp.BLL.Repository;
using DatingApp.BLL.UserManagement;
using DatingApp.DAL;
using DatingApp.DAL.Repository;
using DatingApp.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<LogUserActivity>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMessageGroupService, MessageGroupService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
