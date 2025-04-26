using System.Text.Encodings.Web;
using System.Text.Unicode;
using Application.Extensions;
using Application.MappingProfiles;
using Data.Context;
using Ioc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers and Views
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// AutoMapper
builder.Services.AddAutoMapper(typeof(ProfileMapping).Assembly);

// Encoding
builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(UnicodeRanges.All));

// HttpContext
builder.Services.AddHttpContextAccessor();

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

// Register App Services
builder.Services.AddServices();

// DbContext Configuration
builder.Services.AddDbContext<HesabiqueContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Hesabique")));

var app = builder.Build();

// Middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Custom Routes
app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Database Migration
app.MigrateDatabase<HesabiqueContext>()
    .Run();