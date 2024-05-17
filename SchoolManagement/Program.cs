using Microsoft.EntityFrameworkCore;
using School.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using School.Models.Models;
using School.DataAccess.DbInitializer;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using School.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using School.DataAccess.Repository.IRepository;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => 

 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
 b => b.MigrationsAssembly("School.DataAccess")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    // Configure password settings
    options.Password.RequireDigit = false;         // Set to false to not require a digit
    options.Password.RequireLowercase = false;     // Set to false to not require lowercase characters
    options.Password.RequireUppercase = false;     // Set to false to not require uppercase characters
    options.Password.RequireNonAlphanumeric = false; // Set to false to not require non-alphanumeric characters
    options.Password.RequiredLength = 6;           // Minimum length for passwords
    //options.Password.RequiredUniqueChars = 1;      // Minimum unique characters in a password
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddRazorPages();
// Add TempData configuration
builder.Services.AddMvc().AddSessionStateTempDataProvider();
builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
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

app.UseAuthorization();
SeedDatabase();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Students}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var DbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        DbInitializer.Initialize();
    }
}
