using Microsoft.EntityFrameworkCore;
using Tangy_Business.Repository;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess.Data;
using TangyWeb_Server.Service;
using TangyWeb_Server.Service.IService;
using Syncfusion.Blazor;
using Microsoft.AspNetCore.Identity;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSyncfusionBlazor();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IFileUploadService, FileUploadService>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductPriceRepository, ProductPriceRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Set stripe secret key using the secret key stored securely
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

//Register Syncfusion license
Syncfusion.Licensing.SyncfusionLicenseProvider
    .RegisterLicense("MzAwNjIxMUAzMjM0MmUzMDJlMzBjcDE4OEU3QS9SWk9yQU9kTnE1dXhmNWl3NEk4dTE4RW5tcUpTMEpKZTkwPQ==");


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Seed the database
await SeedDatabase();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();


async Task SeedDatabase()
{
    using var scope = app?.Services.CreateScope();

    var dbInitializer = scope?.ServiceProvider.GetRequiredService<IDbInitializer>();
    await dbInitializer?.Initialize();
}