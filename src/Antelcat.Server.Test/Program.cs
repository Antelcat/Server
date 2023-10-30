using Antelcat.Core.Extensions;
using Antelcat.Core.Models;
using Antelcat.Extensions;
using Antelcat.Server.Extensions;
using Antelcat.Server.Test.Hubs;
using Antelcat.Server.Test.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAntelcatLogger();
// Add services to the container.
builder.Services
    .AddControllers()
    .AddControllersAsServices()
    .UseAutowiredControllers();

builder.Services
    .ConfigureSharedCookie<User>(
        configure: cookie =>
        {
            cookie.HttpOnly = true;
            cookie.Name = $"{nameof(Antelcat)}_{nameof(Antelcat.Server)}";
        },
        denied: static _ => ((Response)"权限不足").Serialize(),
        failed: static _ => ((Response)"未授权").Serialize())
    .ConfigureJwt<User>(
        configure: jwt =>
        {
            var guid = Guid.NewGuid().ToString();
            jwt.Secret = guid;
            Console.WriteLine(guid);
        },
        validation: static (id, context) =>
        {
            if (id.Id < 0)
            {
                context.Fail("Jwt token invalid");
            }
            
            return Task.CompletedTask;
        },
        denied: static _ => ((Response)"权限不足").Serialize(),
        failed: static _ => ((Response)"未授权").Serialize());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
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