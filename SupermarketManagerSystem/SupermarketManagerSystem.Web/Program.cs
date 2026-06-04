using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SupermarketManagerSystem.Data;
using SupermarketManagerSystem.Data.Models;
using SupermarketManagerSystem.Services.Contracts;
using SupermarketManagerSystem.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IRestockRequestService, RestockRequestService>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/500");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

await SeedAsync(app);

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

async Task SeedAsync(WebApplication webApp)
{
    using var scope = webApp.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Roles
    if (!await roleManager.RoleExistsAsync("Administrator"))
        await roleManager.CreateAsync(new IdentityRole("Administrator"));

    // Admin user
    const string adminEmail = "admin@supermarket.bg";
    const string adminPassword = "Admin123!";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "System",
            LastName = "Administrator",
            EmailConfirmed = true,
            RegisteredOn = DateTime.UtcNow
        };
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
            await userManager.AddToRoleAsync(adminUser, "Administrator");
    }

    // Seed demo products only if none exist
    if (!db.Products.Any())
    {
        var catDairy = db.Categories.FirstOrDefault(c => c.Name == "Dairy & Eggs");
        var catMeat = db.Categories.FirstOrDefault(c => c.Name == "Meat & Poultry");
        var catFruits = db.Categories.FirstOrDefault(c => c.Name == "Fruits & Vegetables");
        var catBakery = db.Categories.FirstOrDefault(c => c.Name == "Bakery");
        var catBev = db.Categories.FirstOrDefault(c => c.Name == "Beverages");

        var sup1 = db.Suppliers.FirstOrDefault(s => s.Name == "FreshFarm Ltd.");
        var sup2 = db.Suppliers.FirstOrDefault(s => s.Name == "DairyPro EOOD");
        var sup3 = db.Suppliers.FirstOrDefault(s => s.Name == "MeatMasters AD");

        if (catDairy != null && catMeat != null && catFruits != null &&
            catBakery != null && catBev != null && sup1 != null && sup2 != null && sup3 != null)
        {
            db.Products.AddRange(
                new Product { Name = "Whole Milk 1L", Barcode = "5000000000001", Price = 2.49m, CostPrice = 1.50m, StockQuantity = 120, MinimumStockLevel = 20, Unit = "L", CategoryId = catDairy.Id, SupplierId = sup2.Id, IsActive = true, CreatedOn = DateTime.UtcNow },
                new Product { Name = "Cheddar Cheese 500g", Barcode = "5000000000002", Price = 7.99m, CostPrice = 4.50m, StockQuantity = 45, MinimumStockLevel = 10, Unit = "pcs", CategoryId = catDairy.Id, SupplierId = sup2.Id, IsActive = true, CreatedOn = DateTime.UtcNow },
                new Product { Name = "Free Range Eggs x12", Barcode = "5000000000003", Price = 4.99m, CostPrice = 2.80m, StockQuantity = 8, MinimumStockLevel = 15, Unit = "pcs", CategoryId = catDairy.Id, SupplierId = sup1.Id, IsActive = true, CreatedOn = DateTime.UtcNow },
                new Product { Name = "Chicken Breast 1kg", Barcode = "5000000000004", Price = 9.99m, CostPrice = 6.00m, StockQuantity = 35, MinimumStockLevel = 10, Unit = "kg", CategoryId = catMeat.Id, SupplierId = sup3.Id, IsActive = true, CreatedOn = DateTime.UtcNow },
                new Product { Name = "Pork Mince 500g", Barcode = "5000000000005", Price = 6.49m, CostPrice = 3.80m, StockQuantity = 4, MinimumStockLevel = 10, Unit = "kg", CategoryId = catMeat.Id, SupplierId = sup3.Id, IsActive = true, CreatedOn = DateTime.UtcNow },
                new Product { Name = "Bananas 1kg", Barcode = "5000000000006", Price = 1.99m, CostPrice = 0.90m, StockQuantity = 60, MinimumStockLevel = 20, Unit = "kg", CategoryId = catFruits.Id, SupplierId = sup1.Id, IsActive = true, CreatedOn = DateTime.UtcNow },
                new Product { Name = "Sourdough Bread 800g", Barcode = "5000000000007", Price = 3.49m, CostPrice = 1.80m, StockQuantity = 22, MinimumStockLevel = 10, Unit = "pcs", CategoryId = catBakery.Id, SupplierId = sup1.Id, IsActive = true, CreatedOn = DateTime.UtcNow },
                new Product { Name = "Orange Juice 1L", Barcode = "5000000000008", Price = 2.99m, CostPrice = 1.60m, StockQuantity = 75, MinimumStockLevel = 20, Unit = "L", CategoryId = catBev.Id, SupplierId = sup1.Id, IsActive = true, CreatedOn = DateTime.UtcNow }
            );
            await db.SaveChangesAsync();
        }
    }
}