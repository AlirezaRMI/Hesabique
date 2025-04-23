using System.Text.Encodings.Web;
using System.Text.Unicode;
using Application.Extensions;
using Data.Context;
using Ioc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(UnicodeRanges.All));
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

builder.Services.AddServices();

#region sql config

builder.Services.AddDbContext<HesabiqueContext>
(options => options.UseSqlServer
(builder.Configuration.GetConnectionString
    ("Hesabique")));

#endregion

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MigrateDatabase<HesabiqueContext>()
    .Run();