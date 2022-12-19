using Microsoft.EntityFrameworkCore;
using WatchShop_ASPNETCOREMVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using Microsoft.Extensions.Options;
using Constants = WatchShop_ASPNETCOREMVC.Data.Constants;
using WatchShop_ASPNETCOREMVC.Core.Repositories;
using WatchShop_ASPNETCOREMVC.Repositories;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MvcWatchesConnectionString") ?? throw new InvalidOperationException("Connection string 'MVCWatchesDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MVCWatchesDbContext>(options => options.UseSqlServer(builder.Configuration
    .GetConnectionString("MvcWatchesConnectionString")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MVCWatchesDbContext>();


builder.Services.AddControllersWithViews();

#region Authorization

AddAuthorizationPolicies();

#endregion

AddScoped();

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

app.MapRazorPages();

app.Run();

void AddAuthorizationPolicies() 
{
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));
    });
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(Constants.Policies.RequireAdmin, policy => policy.RequireRole(Constants.Roles.Administrator));
    });
}

void AddScoped()
{
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
}