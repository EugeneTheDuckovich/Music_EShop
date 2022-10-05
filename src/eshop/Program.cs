using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using eshop.Data;
using eshop.Models;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var connectionString = builder.Configuration.GetConnectionString("EShopFullDbContext") ??
                       throw new InvalidOperationException("Connection string 'EShopFullDbContext' not found.");

builder.Services.AddDbContext<EShopFullDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", builder => { builder.RequireClaim(ClaimTypes.Role, "Admin"); });
    options.AddPolicy("User", builder => { builder.RequireClaim(ClaimTypes.Role, "User"); });
});
    builder.Services.AddControllersWithViews();

// Configure session
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-6.0#session-state
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    //options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    // User settings.
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

//builder.Services.ConfigureApplicationCookie(options =>
//{
//	// Cookie settings
//	options.Cookie.HttpOnly = true;
//	options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
//	options.SlidingExpiration = true;
//});
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = "1043487933522-r3l4ajq4djb4gvv1albi570nib117avk.apps.googleusercontent.com";
        options.ClientSecret = "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";
    });
builder.Services.AddAuthentication("Cookie")
    .AddCookie("Cookie", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Index";
    }); 

var application = builder.Build();
if (!application.Environment.IsDevelopment())
{
    application.UseExceptionHandler("/Home/Error");
    application.UseHsts();
}

application.UseHttpsRedirection();
application.UseStaticFiles();
application.UseRouting();
application.UseAuthentication();
application.UseAuthorization();
//application.UseSession();
application.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");
application.Run();
