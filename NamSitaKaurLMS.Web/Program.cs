using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using NamSitaKaurLMS.Application.Abstract;
using NamSitaKaurLMS.Application.Concrete;
using NamSitaKaurLMS.Core.Interfaces;
using NamSitaKaurLMS.Infrastructure.Context;
using NamSitaKaurLMS.Infrastructure.Identity;
using NamSitaKaurLMS.Infrastructure.Repository;
using NamSitaKaurLMS.WebUI.Areas.Admin.Controllers;
using NamSitaKaurLMS.WebUI.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AdminAreaAuthorization>();
});
builder.Services.AddDbContext<NamSitaKaurLMSContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

// ASP.NET Core Identity Entegrasyonu
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 6;
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

})
.AddEntityFrameworkStores<NamSitaKaurLMSContext>()
.AddDefaultTokenProviders();


// Generic repository injection
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(ICourseRepository), typeof(CourseRepository));
builder.Services.AddScoped(typeof(ICourseService), typeof(CourseService));
// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Areas", "Admin", "Static")),
    RequestPath = "/admin-static"
});
// Seed Identity Data For Admin User and Roles
await IdentitySeed.SeedAsync(app.Services);

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
