using Antelcat.Core.Extensions;
using Antelcat.Extensions;
using Antelcat.Server.Extensions;
using Antelcat.Server.Test.Hubs;
using Antelcat.Server.Test.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAntelcatLogger(true);
// Add services to the container.
builder.Services
    .AddAntelcatControllers()
    .AddControllersAsServices()
    .UseAutowiredControllers();

builder.Services
    .ConfigureSharedCookie(
        configure: cookie =>
        {
            cookie.HttpOnly = true;
            cookie.Name     = $"{nameof(Antelcat)}_{nameof(Antelcat.Server)}";
            cookie.OnDenied = static _ => ((HttpPayload)"权限不足").Serialize();
            cookie.OnFailed = static _ => ((HttpPayload)"未授权").Serialize();
        })
    .ConfigureJwt(
        configure: jwt =>
        {
            jwt.Secret      = Guid.NewGuid().ToString();
            jwt.OnValidated = static _ => Task.CompletedTask;
            jwt.OnForbidden = static _ => ((HttpPayload)"权限不足").Serialize();
            jwt.OnFailed    = static _ => ((HttpPayload)"未授权").Serialize();
        });
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