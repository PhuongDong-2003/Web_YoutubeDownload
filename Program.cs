using Auth0.AspNetCore.Authentication;
using DownloadYoutube.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using Serilog.Filters;
using Serilog.Formatting.Compact;
using YoutubeDownload.Middleware;
using YoutubeDownload.Support;

Serilog.Log.Logger = new LoggerConfiguration()
    .Enrich.WithProperty("Application", "YoutubeDownLoad")
    .Enrich.FromLogContext()
    .Filter.ByExcluding(le => Matching.FromSource("System")(le))
    .Filter.ByIncludingOnly(le =>
    {
        if (Matching.FromSource("Microsoft")(le))
        {
            return false;
        }
        return true;
    })

    .WriteTo.Console(
        outputTemplate: "{Application} | {Timestamp:HH:mm:ss} | {Name} | {Level} | {SourceContext} | {Message:lj} {NewLine}{Exception}")
    .WriteTo.File(new CompactJsonFormatter(), "log.txt")
    .CreateLogger();
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddScoped<DownLoadYoutubeService>();
builder.Services.AddScoped<FindService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging(l =>
{
    l.ClearProviders();
    l.AddSerilog();
});

builder.Services.AddAuth0WebAppAuthentication(options =>
{

    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    options.CallbackPath = "/auth0/callback";


});

builder.Services.ConfigureSameSiteNoneCookies();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseRouting();

app.UseAuthentication();
app.UseMiddleware<LogContextMiddleware>();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

