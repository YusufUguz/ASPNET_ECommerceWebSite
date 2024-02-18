using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ETemizlik.Data;
using ETemizlik.Models;
using ETemizlik.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using AuthUseContext1 = ETemizlik.Models.AuthUseContext;
using AuthUseContext2 = ETemizlik.Data.AuthUseContext;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AuthUseContextConnection") ?? throw new InvalidOperationException("Connection string 'AuthUseContextConnection' not found.");

builder.Services.AddDbContext<AuthUseContext2>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AuthUseUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<AuthUseContext2>();

builder.Services.AddDbContext<AuthUseContext1>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("constring")));

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<AuthUseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("constring")));

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
app.UseAuthentication();;

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
            name: "Identity",
            pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}");
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
