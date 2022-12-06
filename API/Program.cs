using DatingApp.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DatingApp.FrontEndAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string databaseName = Profile._containerName;
            var connectionString = builder.Configuration.GetConnectionString("CosmosDb");
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<ProfileContext>(opt =>
            {
                opt.UseCosmos(connectionString, databaseName);
            });

            builder.Services.AddCors();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors(b => b.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

            app.MapControllers();

            app.Run();
        }
    }
}