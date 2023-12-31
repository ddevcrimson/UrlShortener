using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;
using UrlShortener.Services;
using UrlShortener.Utils;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(AppMappingProfile));
builder.Services.AddRateLimiter(_ => _.AddFixedWindowLimiter("fixed", o =>
{
    o.PermitLimit = 1;
    o.Window = TimeSpan.FromSeconds(1);
}));
builder.Services.AddDbContext<ApplicationContext>(_ =>
{
    var conf = builder.Configuration.GetConnectionString("MySQL");
    if (string.IsNullOrEmpty(conf)) throw new Exception("MySQL connection string is empty");
    _.UseMySQL(conf);
});
builder.Services.AddScoped<ILinksService, LinksService>();
builder.Services.AddExpiredLinksWorker(new()
{
    LinkTTL = TimeSpan.FromDays(1),
    RunEvery = TimeSpan.FromHours(1)
});

var app = builder.Build();
app.UseRateLimiter();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
