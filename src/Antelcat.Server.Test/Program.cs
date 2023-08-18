using Antelcat.Core.Extensions;
using Antelcat.Extensions;
using Antelcat.Server.Extensions;
using Antelcat.Server.Test.Hubs;
using Antelcat.Server.Test.Models;

namespace Antelcat.Server.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAntelcatLogger();
            // Add services to the container.
            builder.Services
                .AddControllers()
                .AddControllersAsServices()
                .UseAutowiredControllers();
            builder.Services.ConfigureJwt<User>(configure: jwt =>
                {
                    jwt.Secret = Guid.NewGuid().ToString();
                },
                validation: static async (id,context) => {
                    if (id.Id < 0)
                    {
                       context.Fail("Jwt token invalid"); 
                    }  
                },
            failed: static _ => @"{ ""?"" : ""?"" }");
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddJwtSwaggerGen();
            builder.Services.AddSignalR();
            builder.Host.UseAutowiredServiceProviderFactory();
            
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<StreamHub>("/stream");
            app.Run();
        }
    }
}