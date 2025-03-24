using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Data;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = Environment.GetEnvironmentVariable("RdsConnectionString") 
                       ?? builder.Configuration.GetConnectionString("RdsConnectionString") 
                       ?? throw new InvalidOperationException("Connection string 'RdsConnectionString' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

//session

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(1000);
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//Stripe configuration
StripeConfiguration.ApiKey =
    "sk_test_51R5zvrHcMSUslYQYetHTnJijC1m9r6HiKfn7PyRmlfFZZsiTzmcQ5AnfhYWtrmBsp1XDMpEYWfZNWvju8t7dMset00Tym1x09H";
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Shop}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

app.Run();