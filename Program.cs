using Auth0.AspNetCore.Authentication;
using DownloadYoutube.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using YoutubeDownload.Support;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<DownLoadYoutubeService>();
builder.Services.AddScoped<FindService>();

builder.Services.AddAuth0WebAppAuthentication(options =>
{
   
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    // options.CallbackPath = "/Account/callback";
    
});

// Configure the HTTP request pipeline.
builder.Services.ConfigureSameSiteNoneCookies();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
