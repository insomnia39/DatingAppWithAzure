using DatingApp.FrontEndAPI.Extensions;
using DatingApp.FrontEndAPI.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.FrontEndAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();
            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            app.UseCors(b => b.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            //try
            //{
            //    var context = services.GetRequiredService<ProfileContext>();
            //    await Seed.SeedUser(context);
            //}
            //catch (Exception e)
            //{
            //    var logger = services.GetService<ILogger<Program>>();
            //    logger.LogError(e, "Error Occured");
            //}

            app.Run();
        }
    }
}