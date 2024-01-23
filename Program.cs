using Auth0.AspNetCore.Authentication;
using DownloadYoutube.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using YoutubeDownload.Support;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddScoped<DownLoadYoutubeService>();
builder.Services.AddScoped<FindService>();

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
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

